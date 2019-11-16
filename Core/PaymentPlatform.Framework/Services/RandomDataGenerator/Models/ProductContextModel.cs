using PaymentPlatform.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Models
{
    /// <summary>
    /// Модель товара для Context.
    /// </summary>
    [Table("Product")]
    public class ProductContextModel : ProductModel
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        // Навигационные свойства.
        public ProfileContextModel Profile { get; set; }

        public IEnumerable<TransactionContextModel> Transactions { get; set; }
    }
}