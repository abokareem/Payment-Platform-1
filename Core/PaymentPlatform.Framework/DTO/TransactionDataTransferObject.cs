using System;

namespace PaymentPlatform.Framework.DTO
{
    /// <summary>
    /// Модель данных для транспортироваки Transaction.
    /// </summary>
    public class TransactionDataTransferObject
    {
        /// <summary>
        /// Идентификатор покупателя.
        /// </summary>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Идентификатор продукта.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Итоговая стоймость.
        /// </summary>
        public decimal Cost { get; set; }
    }
}
