using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// ViewModel для транзакции.
    /// </summary>
    public class TransactionViewModel : IHasGuidIdentity
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
        public DateTime TransactionTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Статус.
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Итоговая стоймость.
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// Общее количество.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
