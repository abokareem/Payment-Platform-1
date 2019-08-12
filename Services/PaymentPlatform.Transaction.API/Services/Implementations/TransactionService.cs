using PaymentPlatform.Transaction.API.Services.Interfaces;
using PaymentPlatform.Transaction.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PaymentPlatform.Transaction.API.Models;
using AutoMapper;

namespace PaymentPlatform.Transaction.API.Services.Implementations
{
	public class TransactionService : ITransactionService
	{
		private readonly TransactionContext _transactionContext;
		private readonly IMapper _mapper;
		public TransactionService(TransactionContext transactionContext, IMapper mapper)
		{
			_transactionContext = transactionContext;
			_mapper = mapper;
		}

		public Task<(bool success, string result)> AddNewTransactionAsync(TransactionViewModel transaction)
		{
			throw new NotImplementedException();
		}

		private Task MakeReservesAsync(Models.Transaction transaction)
		{
			throw new NotImplementedException();
		}

		private Task CancelReserveAsync(Models.Transaction transaction)
		{
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
