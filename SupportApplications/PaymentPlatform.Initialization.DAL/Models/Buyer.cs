using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель покупателя
	/// </summary>
	public class Buyer
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Счет
		/// </summary>
		public string Billing { get; set; }
		/// <summary>
		/// Баланс
		/// </summary>
		public decimal Balance { get; set; }

		public List<Transaction> Transactions { get; set; }
		public List<Customer> Customers { get; set; }
	}
}
