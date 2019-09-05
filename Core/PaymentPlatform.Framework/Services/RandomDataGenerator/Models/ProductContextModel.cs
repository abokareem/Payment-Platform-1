using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
	/// Модель товара для Context.
	/// </summary>
    [Table("Products")]
    public class ProductContextModel : ProductModel
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        // Навигационные свойства.
        public ProfileContextModel Profile { get; set; }
        public ICollection<TransactionContextModel> Transactions { get; set; }
        public ICollection<ProductReservedContextModel> ProductReserveds { get; set; }
    }
}
