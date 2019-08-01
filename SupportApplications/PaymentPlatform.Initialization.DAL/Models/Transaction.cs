using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель транзакции.
	/// </summary>
	public class Transaction
	{
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
		/// Количество продуктов.
		/// </summary>
		public int ProductCount { get; set; }

		/// <summary>
		/// Итоговая стоимость.
		/// </summary>
		public decimal Total { get; set; }

		public Product Product { get; set; }
		public Profile Profile { get; set; }
	}
}
