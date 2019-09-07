using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
	/// Модель транзакции.
	/// </summary>
    [Table("Transactions")]
    public class TransactionModel
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
        public int Status { get; set; }

        /// <summary>
        /// Итоговая стоймость.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Идентификатор резерва денег.
        /// </summary>
        public Guid? BalanceReserveId { get; set; }
        /// <summary>
        /// Идентификатор резерва товара.
        /// </summary>
        public Guid? ProductReserveId { get; set; }

        /// <summary>
        /// Успешность транзакции на основе резерва.
        /// </summary>
        public bool TransactionSuccess { get; set; }
    }
}
