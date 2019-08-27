using PaymentPlatform.Transaction.API.Services.Interfaces;
using PaymentPlatform.Transaction.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentPlatform.Transaction.API.Models;
using AutoMapper;
using PaymentPlatform.Core.Interfaces;
using Newtonsoft.Json;
using PaymentPlatform.Core.Models;
using PaymentPlatform.Core.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Transaction.API.Services.Implementations
{
	public class TransactionService : ITransactionService
	{
		private readonly TransactionContext _transactionContext;
		private readonly IMapper _mapper;
		private readonly IRabbitService _rabbitService;

		public TransactionService(TransactionContext transactionContext, IMapper mapper, IRabbitService rabbitService)
		{
			_transactionContext = transactionContext;
			_mapper = mapper;
			_rabbitService = rabbitService;
			_rabbitService.SetListener("TransactionAPI", OnIncomingMessage);
		}


		private void OnIncomingMessage(string message)
		{
			//TODO: TryParse incoming message
			try
			{
				var incomingMessage = JsonConvert.DeserializeObject(message) as RabbitMessage;

				switch (incomingMessage.Sender)
				{
					case "ProductAPI":
						{
							var productReserve = incomingMessage.Model as ProductReserve;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == productReserve.TransactionId);
							transaction.ProductReserveId = productReserve.Id;
							_transactionContext.Update(transaction);
							_transactionContext.SaveChanges();
							break;
						}
					case "ProfileAPI":
						{
							var balanceReserve = incomingMessage.Model as BalanceReserve;
							var transaction = _transactionContext.Transactions.FirstOrDefault(t => t.Id == balanceReserve.TransactionId);
							transaction.BalanceReserveId = balanceReserve.Id;
							if (transaction.BalanceReserve != null && transaction.ProductReserveId != null)
							{
								transaction.TransactionSuccess = true;
							}
							_transactionContext.Update(transaction);
							_transactionContext.SaveChanges();
							break;
						}
					default:
						throw new JsonException("Unable to parse JSON.");
				}

			}
			catch (JsonException jsonExc)
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
			var newTransaction = new Core.Models.DatabaseModels.Transaction { };
			_transactionContext.Transactions.Add(newTransaction);
			await _transactionContext.SaveChangesAsync();
			MakeReserve(newTransaction);
			return (true, "Добавлена новая транзакция.");
		}

		private void MakeReserve(Core.Models.DatabaseModels.Transaction transaction)
		{
			var messageToProfile = new RabbitMessage { Action = "Apply", Sender = "TransactionAPI", Model = _mapper.Map<BalanceReserve>(transaction) };
			var messageToProduct = new RabbitMessage { Action = "Apply", Sender = "TransactionAPI", Model = _mapper.Map<ProductReserve>(transaction) };

			//TODO: Decrease balance
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile),"ProfileAPI");
			//TODO: Decrease product
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		private void RevertReserve(Core.Models.DatabaseModels.Transaction transaction)
		{
			var messageToProfile = new RabbitMessage { Action = "Revert", Sender = "TransactionAPI", Model = _mapper.Map<BalanceReserve>(transaction) };
			var messageToProduct = new RabbitMessage { Action = "Revert", Sender = "TransactionAPI", Model = _mapper.Map<ProductReserve>(transaction) };

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
			return _mapper.Map<List<TransactionViewModel>>(transactions);
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
			//TODO: Не успел
		}

	}
}
