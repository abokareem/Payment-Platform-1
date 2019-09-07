using PaymentPlatform.Transaction.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Services.Interfaces
{
	public interface ITransactionService
	{
		Task<ICollection<TransactionViewModel>> GetTransactionsAsync(int? take = null, int? skip = null);
		Task<TransactionViewModel> GetTransactionByIdAsync(Guid id);
		Task<(bool success, string result)> AddNewTransactionAsync(TransactionViewModel transaction);
		Task<(bool success, string result)> RevertTransactionByIdAsync(Guid id);
		Task<TransactionViewModel> UpdateTransactionAsync(TransactionViewModel transaction); 
	}
}
