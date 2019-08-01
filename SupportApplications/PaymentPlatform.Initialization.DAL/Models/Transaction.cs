using System;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель транзакции.
	/// </summary>
	public class Transaction
	{
        /// <summary>
        /// Идентификатор (уникальный номер транзакции) (GUID).
        /// </summary>
        public string Id { get; set; }

		/// <summary>
		/// Идентификатор покупателя.
		/// </summary>
		public string CustomerId { get; set; }

		/// <summary>
		/// Идентификатор продукта.
		/// </summary>
		public string ProductId { get; set; }

        /// <summary>
        /// Время совершения операции.
        /// </summary>
        public DateTime TransictionTime { get; set; } = DateTime.Now;

		/// <summary>
		/// Статус.
		/// </summary>
		public int Status { get; set; }

		/// <summary>
		/// Итоговая стоимость.
		/// </summary>
		public decimal Total { get; set; }
										   // TODO: Добавить связи через ICollection
	}
}
