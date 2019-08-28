using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Core.Models.DatabaseModels
{
	public class BalanceReserve
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Идентификатор транзакции.
		/// </summary>
		public Guid TransactionId { get; set; }

		/// <summary>
		/// Идентификатор профиля.
		/// </summary>
		public Guid ProfileId { get; set; }

		/// <summary>
		/// Полная сумма.
		/// </summary>
		public decimal Total { get; set; }

		/// <summary>
		/// Дата создания резерва.
		/// </summary>
		public DateTime ReservationDate { get; set; }

		/// <summary>
		/// Статус.
		/// </summary>
		public int Status { get; set; }

		// Навигационные свойства
		public Profile Profile { get; set; }
		public Transaction Transaction { get; set; }
	}
}
