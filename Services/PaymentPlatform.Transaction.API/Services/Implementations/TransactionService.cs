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
						var productReserve = incomingMessage.Model as ProductReserve;
						var transaction = _transactionContext.Transactions.FirstOrDefault(productReserve.)
						break;
					case "ProfileAPI":
						var balanceReserve = incomingMessage.Model as BalanceReserve;
						break;
					default:
						break;
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
			var newTransaction = new Models.Transaction { };
			_transactionContext.Transactions.Add(newTransaction);
			await _transactionContext.SaveChangesAsync();
			MakeReserve(newTransaction);
			return (true, "Добавлена новая транзакция.");
		}

		private void MakeReserve(Models.Transaction transaction)
		{
			var messageToProfile = new RabbitMessage { Action = "Apply", Sender = "TransactionAPI", Model = _mapper.Map<BalanceReserve>(transaction) };
			var messageToProduct = new RabbitMessage { Action = "Apply", Sender = "TransactionAPI", Model = _mapper.Map<ProductReserve>(transaction) };

			//TODO: Decrease balance
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProfile),"ProfileAPI");
			//TODO: Decrease product
			_rabbitService.SendMessage(JsonConvert.SerializeObject(messageToProduct), "ProductAPI");
		}

		private Task RevertReserveAsync(Models.Transaction transaction)
		{
			throw new NotImplementedException();
		}
		public async Task<TransactionViewModel> GetTransactionByIdAsync(Guid id)
		{
			var transaction = await _transactionContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
			return _mapper.Map<TransactionViewModel>(transaction);
		}

		public Task<ICollection<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null)
		{
			throw new NotImplementedException();
		}

		public Task<(bool success, string message)> RevertTransactionByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionViewModel> UpdateTransactionAsync(TransactionViewModel transaction)
		{
			throw new NotImplementedException();
		}

	}
}
