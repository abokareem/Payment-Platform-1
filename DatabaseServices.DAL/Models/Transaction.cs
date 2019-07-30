using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL.Models
{
	/// <summary>
	/// Модель транзакции
	/// </summary>
	public class Transaction
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Уникальный хэш-код
		/// </summary>
		public string UniqueHashNumber { get; set; }
		/// <summary>
		/// Идентификатор продавца
		/// </summary>
		public int SellerId { get; set; }
		/// <summary>
		/// Идентификатор покупателя
		/// </summary>
		public int BuyerId { get; set; }
		/// <summary>
		/// Идентификатор продукта
		/// </summary>
		public int ProductId { get; set; }
		/// <summary>
		/// Время транзакции
		/// </summary>
		public DateTime TransactionTime { get; set; }
		/// <summary>
		/// Статус транзакции
		/// </summary>
		public int TransactionStatus { get; set; }

		public Seller Seller { get; set; }
		public Buyer Buyer { get; set; }
		public Product Product { get; set; }
	}
}
