using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель продавца
	/// </summary>
	public class Seller
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Название организации
		/// </summary>
		public string OrganisationName { get; set; }
		/// <summary>
		/// Номер организации (УИН, ИНН)
		/// </summary>
		public string OrganisationNumber { get; set; }
		/// <summary>
		/// Ответственное лицо
		/// </summary>
		public string ResponsiblePerson { get; set; }
		/// <summary>
		/// Счет
		/// </summary>
		public string Billing { get; set; }
		/// <summary>
		/// Баланс
		/// </summary>
		public decimal Balance { get; set; }


		public List<Product> Products { get; set; }
		public List<Transaction> Transactions { get; set; }
		public List<Customer> Customers { get; set; }
	}
}
