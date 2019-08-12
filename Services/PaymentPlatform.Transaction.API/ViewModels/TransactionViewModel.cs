using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.ViewModels
{
	public class TransactionViewModel
	{
		public Guid Id { get; set; }

		/// <summary>
		/// Идентификатор покупателя.
		/// </summary>
		public Guid ProfileId { get; set; }

		/// <summary>
		/// Идентификатор продукта.
		/// </summary>
		public Guid ProductId { get; set; }

		/// <summary>
		/// Время совершения операции.
		/// </summary>
		public DateTime TransactionTime { get; set; } = DateTime.Now;

		/// <summary>
		/// Статус.
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Итоговая стоймость.
		/// </summary>
		public decimal TotalCost { get; set; }
	}
}
