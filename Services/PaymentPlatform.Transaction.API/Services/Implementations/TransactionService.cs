using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentPlatform.Framework.Constants.Logger;
using PaymentPlatform.Framework.DTO;
using PaymentPlatform.Framework.Enums;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Services.Implementations
{
    /// <summary>
    /// Сервис транзакций.
    /// </summary>
    public class TransactionService : ITransactionService
    {
        /// <summary>
        /// Контекст транзакций.
        /// </summary>
        private readonly TransactionContext _transactionContext;

        /// <summary>
        /// Экземпляр автомаппера.
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Сервис брокера сообщений.
        /// </summary>
        private readonly IRabbitMQService _rabbitService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="transactionContext">Контекст транзакций.</param>
        /// <param name="mapper">Экземпляр автомаппера.</param>
        /// <param name="rabbitService">Сервис брокера сообщений.</param>
        public TransactionService(TransactionContext transactionContext,
                                  IMapper mapper,
                                  IRabbitMQService rabbitService)
        {
            _transactionContext = transactionContext ?? throw new ArgumentException(nameof(transactionContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _rabbitService = rabbitService ?? throw new ArgumentException(nameof(rabbitService));

            _rabbitService.ConfigureServiceDefault();
        }

        /// <inheritdoc/>
        public async Task<(bool success, string message)> AddNewTransactionAsync(TransactionViewModel transaction)
        {
            var transactionModel = _mapper.Map<TransactionModel>(transaction);

            MakeReserve(transactionModel);

            await _transactionContext.Transactions.AddAsync(transactionModel);
            await _transactionContext.SaveChangesAsync();

            return (true, $"{transaction.Id} {TransactionLoggerConstants.ADD_TRANSACTION_OK}");
        }

        /// <summary>
        /// Создает резерв товара и средств со счета пользователя.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        private void MakeReserve(TransactionModel transaction)
        {
            var transactionDTO = new TransactionDataTransferObject
            {
                ProfileId = transaction.ProfileId,
                ProductId = transaction.ProductId,
                Cost = transaction.TotalCost
            };

            var messageToProfile = new RabbitMessageModel { Action = (int)RabbitMessageActions.Apply, Sender = "TransactionAPI", Model = transactionDTO };
            var messageToProduct = new RabbitMessageModel { Action = (int)RabbitMessageActions.Apply, Sender = "TransactionAPI", Model = transactionDTO };

            _rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
            _rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile), "ProfileAPI");
        }

        /// <summary>
        /// Отменяет резеврирование товара и средств пользователя.
        /// </summary>
        /// <param name="transaction">Транзакция.</param>
        private void RevertReserve(TransactionModel transaction)
        {
            var transactionDTO = new TransactionDataTransferObject
            {
                ProfileId = transaction.ProfileId,
                ProductId = transaction.ProductId,
                Cost = transaction.TotalCost
            };

            var messageToProfile = new RabbitMessageModel { Action = (int)RabbitMessageActions.Revert, Sender = "TransactionAPI", Model = transactionDTO };
            var messageToProduct = new RabbitMessageModel { Action = (int)RabbitMessageActions.Revert, Sender = "TransactionAPI", Model = transactionDTO };

            _rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile), "ProfileAPI");
            _rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
        }

        /// <inheritdoc/>
        public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid id)
        {
            var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

            return _mapper.Map<TransactionViewModel>(transaction);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null)
        {
            var transactions = _transactionContext.Transactions.Select(t => t);

            if (take > 0)
            {
                transactions = transactions.Take((int)take);
            }

            if (skip > 0)
            {
                transactions = transactions.Skip((int)skip);
            }

            var transactionResult = await transactions.ToListAsync();
            var transactionResultViewModels = _mapper.Map<List<TransactionViewModel>>(transactionResult);

            return transactionResultViewModels;
        }

        /// <inheritdoc/>
        public async Task<(bool success, string message)> RevertTransactionByIdAsync(Guid id)
        {
            var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

            RevertReserve(transaction);

            transaction.Status = 0;
            var transactionViewModel = _mapper.Map<TransactionViewModel>(transaction);
            await UpdateTransactionAsync(transactionViewModel);

            return (true, $"{id} {TransactionLoggerConstants.REVERT_TRANSACTION_OK}");
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateTransactionAsync(TransactionViewModel updatedTransaction)
        {
            var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == updatedTransaction.Id);

            if (transaction == null)
            {
                return false;
            }

            transaction.ProductId = updatedTransaction.ProductId;
            transaction.ProfileId = updatedTransaction.ProfileId;
            transaction.TransactionTime = updatedTransaction.TransactionTime;
            transaction.Status = updatedTransaction.Status;
            transaction.TotalCost = updatedTransaction.TotalCost;

            _transactionContext.Transactions.Update(transaction);
            await _transactionContext.SaveChangesAsync();

            return true;
        }
    }
}