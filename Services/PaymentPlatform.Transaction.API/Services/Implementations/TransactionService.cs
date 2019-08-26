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
			_rabbitService.SetListener("TransactionAPI", OnIncomingMessage(""));
		}

		private Action<string> OnIncomingMessage(string v)
		{
			//TODO: ParseMessage
			return TODOSomething;
		}

		private void TODOSomething (string str)
		{

		}

		public Task<(bool success, string result)> AddNewTransactionAsync(TransactionViewModel transaction)
		{
			throw new NotImplementedException();
		}

		private async Task<bool> MakeReservesAsync(Models.Transaction transaction)
		{
			var balanceReserve = _mapper.Map<BalanceReserve>(transaction);
			var productReserve = _mapper.Map<ProductReserve>(transaction);

			//TODO: Decrease balance
			_rabbitService.SendMessage(JsonConvert.SerializeObject(new BalanceReserve { }),"ProfileAPI");
			//TODO: Decrease product
			await _transactionContext.BalanceReserves.AddAsync(balanceReserve);
			await _transactionContext.ProductReserves.AddAsync(productReserve);
			
			await _transactionContext.SaveChangesAsync();

			if (balanceReserve.Id != null && productReserve.Id != null)
			{
				var newTransaction = _mapper.Map<Models.Transaction>(transaction);
				await _transactionContext.Transactions.AddAsync(newTransaction);
				return true;
			}
			else
			{
				return false;
			}
		}

		private Task CancelReserveAsync(Models.Transaction transaction)
		{
			var balanceReserve = _transactionContext.BalanceReserves.FirstOrDefault(br => br.Id == transaction.BalanceReserveId);
			var productReserve = _transactionContext.ProductReserves.FirstOrDefault(br => br.Id == transaction.ProductReserveId);

			if (balanceReserve != null)
			{
				//TODO: Increate balance
			}

			if (productReserve != null)
			{
				//TODO: Increate product
			}
			throw new NotImplementedException();
		}
		public Task<TransactionViewModel> GetTransactionByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<ICollection<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null)
		{
			throw new NotImplementedException();
		}

		public Task<(bool success, string result)> RevertTransactionByIdAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Task<TransactionViewModel> UpdateTransactionAsync(TransactionViewModel transaction)
		{
			throw new NotImplementedException();
		}

	}
}
