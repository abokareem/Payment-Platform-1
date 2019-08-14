using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Transaction.API.Models
{
	public class BalanceReserve
	{
		/// <summary>
		/// Идентификатор.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Идентификатор профиля.
		/// </summary>
		public Guid ProfileId { get; set; }

		/// <summary>
		/// Полная сумма.
		/// </summary>
		public float Total { get; set; }

		/// <summary>
		/// Дата создания резерва.
		/// </summary>
		public DateTime ReservationDate { get; set; }

		/// <summary>
		/// Успешность транзакции на основе резерва.
		/// </summary>
		public bool TransactionSuccess { get; set; }

		// Навигационные свойства
		public Transaction Transaction { get; set; }
	}
}
