using PaymentPlatform.Framework.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Models
{
    /// <summary>
    /// Модель профиля пользователя для Context.
    /// </summary>
    [Table("Profile")]
    public class ProfileContextModel : ProfileModel
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key]
        public new Guid Id { get; set; }

        // Навигационные свойства.
        public AccountContextModel Account { get; set; }

        public IEnumerable<TransactionContextModel> Transactions { get; set; }
        public IEnumerable<ProductContextModel> Products { get; set; }
    }
}