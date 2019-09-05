using System;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель зарезервированного продукта для Context.
    /// </summary>
    public class ProductReservedContextModel : ProductReservedModel
    {
        // Навигационные свойства
        public ProductContextModel Product { get; set; }
        public TransactionContextModel Transaction { get; set; }
    }
}
