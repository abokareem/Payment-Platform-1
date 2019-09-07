using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL.Models
{
	public class ProductReserve
	{
		/// <summary>
		/// Идентификатор резерва.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Идентификатор товара.
		/// </summary>
		public Guid ProductId { get; set; }

		/// <summary>
		/// Полная стоимость.
		/// </summary>
		public float TotalCost { get; set; }

		/// <summary>
		/// Количество.
		/// </summary>
		public int Amount { get; set; }

		/// <summary>
		/// Дата создания резерва.
		/// </summary>
		public DateTime ReservationDate { get; set; }

		/// <summary>
		/// Успешность транзакции на основе резерва.
		/// </summary>
		public bool TransactionSuccess { get; set; }

		// Навигационные свойства

		public Product Product { get; set; }
		public Transaction Transaction { get; set; }
	}
}
