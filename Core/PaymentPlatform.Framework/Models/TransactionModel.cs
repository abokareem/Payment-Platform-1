using PaymentPlatform.Framework.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель транзакции.
    /// </summary>
    [Table("Transaction")]
    public class TransactionModel : IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
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
        public DateTime TransactionTime { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Итоговая стоймость.
        /// </summary>
        public decimal TotalCost { get; set; }
    }
}