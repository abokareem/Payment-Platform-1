using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель зарезервированного баланса.
    /// </summary>
    public class BalanceReservedModel : IHasGuidIdentity
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
    }
}