using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель зарезервированного продукта.
    /// </summary>
    public class ProductReservedModel : IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор резерва.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Идентификатор транзакции.
        /// </summary>
        public Guid TransactionId { get; set; }

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
        /// Статус.
        /// </summary>
        public int Status { get; set; }
    }
}