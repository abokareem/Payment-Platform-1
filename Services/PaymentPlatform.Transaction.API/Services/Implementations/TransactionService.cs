using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentPlatform.Transaction.API.Models;
using AutoMapper;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Transaction.API.Services.Implementations
{
	public class TransactionService : ITransactionService
	{
		private readonly TransactionContext _transactionContext;
		private readonly IMapper _mapper;
		private readonly IRabbitMQService _rabbitService;

		public TransactionService(TransactionContext transactionContext, IMapper mapper, IRabbitMQService rabbitService)
		{
			_transactionContext = transactionContext;
			_mapper = mapper;
			_rabbitService = rabbitService;
			_rabbitService.SetListener("TransactionAPI", OnIncomingMessage);
		}


		private void OnIncomingMessage(string message)
		{
			try
			{
				var incomingMessage = JsonConvert.DeserializeObject(message) as RabbitMessageModel;

				switch (incomingMessage.Sender)
				{
					case "ProductAPI":
						{
							var productReserve = incomingMessage.Model as ProductReservedModel;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == productReserve.TransactionId);

                            var incomingRabbitMessage = string.Empty;
                            incomingRabbitMessage = incomingMessage.Action;

                            switch (incomingRabbitMessage)
                            {
                                case "Apply":
                                    {
                                        transaction.ProductReserveId = productReserve.Id;

                                        if (transaction.BalanceReserveId != null && transaction.ProductReserveId != null)
                                        {
                                            transaction.TransactionSuccess = true;
                                        }
                                    }
                                    break;

                                case "Revert":
                                    {
                                        transaction.TransactionSuccess = false;
                                    }
                                    break;

                                default:
                                    {
                                        throw new JsonException("Unexpected action.");
                                    }
                            }

							_transactionContext.Entry(transaction).State = EntityState.Modified;
							_transactionContext.SaveChanges();

							break;
						}
					case "ProfileAPI":
						{
							var balanceReserve = incomingMessage.Model as BalanceReservedModel;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == balanceReserve.TransactionId);

                            var incomingRabbitMessage = string.Empty;
                            incomingRabbitMessage = incomingMessage.Action;

                            switch (incomingRabbitMessage)
                            {
                                case "Apply":
                                    {
                                        transaction.BalanceReserveId = balanceReserve.Id;

                                        if (transaction.BalanceReserveId != null && transaction.ProductReserveId != null)
                                        {
                                            transaction.TransactionSuccess = true;
                                        }
                                    }
                                    break;

                                case "Revert":
                                    {
                                        transaction.TransactionSuccess = false;
                                    }
                                    break;

                                default:
                                    {
                                        throw new JsonException("Unexpected action.");
                                    }
                            }

							_transactionContext.Entry(transaction).State = EntityState.Modified;
							_transactionContext.SaveChanges();

							break;
						}

					default:
						throw new JsonException("Unexpected sender.");
				}
			}
			catch (JsonException)
			{
				//TODO: Вывести в лог
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}

		public async Task<(bool success, string message)> AddNewTransactionAsync(TransactionViewModel transaction)
		{
			var newTransaction = _mapper.Map<TransactionModel>(transaction);
			_transactionContext.Entry(newTransaction).State = EntityState.Added;

			await _transactionContext.SaveChangesAsync();
			MakeReserve(newTransaction);

			return (true, "Добавлена новая транзакция.");
		}

		private void MakeReserve(TransactionModel transaction)
		{
            var balanceReserveModel = _mapper.Map<BalanceReservedModel>(transaction);
            var productReserveModel = _mapper.Map<ProductReservedModel>(transaction);

            var messageToProfile = new RabbitMessageModel { Action = "Apply", Sender = "TransactionAPI", Model = balanceReserveModel };
			var messageToProduct = new RabbitMessageModel { Action = "Apply", Sender = "TransactionAPI", Model = productReserveModel };

			//TODO: Decrease balance
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile),"ProfileAPI");
			//TODO: Decrease product
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		private void RevertReserve(TransactionModel transaction)
		{
            var balanceReserveModel = _mapper.Map<BalanceReservedModel>(transaction);
            var productReserveModel = _mapper.Map<ProductReservedModel>(transaction);

            var messageToProfile = new RabbitMessageModel { Action = "Revert", Sender = "TransactionAPI", Model = balanceReserveModel };
			var messageToProduct = new RabbitMessageModel { Action = "Revert", Sender = "TransactionAPI", Model = productReserveModel };

			//TODO: Decrease balance
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile), "ProfileAPI");
			//TODO: Decrease product
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid id)
		{
			var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

			return _mapper.Map<TransactionViewModel>(transaction);
		}

		public async Task<ICollection<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null)
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

		public async Task<(bool success, string message)> RevertTransactionByIdAsync(Guid id)
		{
			var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);

			RevertReserve(transaction);
			transaction.TransactionSuccess = false;

			return (true, "Transaction canceled successfully.");
		}

		public async Task<TransactionViewModel> UpdateTransactionAsync(TransactionViewModel transaction)
		{
			var transactionInDatabase = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);

			transactionInDatabase.ProductId = transaction.ProductId;
			transactionInDatabase.ProfileId = transaction.ProfileId;
			transactionInDatabase.TransactionTime = transaction.TransactionTime;
			transactionInDatabase.Status = transaction.Status;
			transactionInDatabase.TotalCost = transaction.TotalCost;

			_transactionContext.Transactions.Update(transactionInDatabase);
			await _transactionContext.SaveChangesAsync();

			return _mapper.Map<TransactionViewModel>(transactionInDatabase);
		}

	}
}
