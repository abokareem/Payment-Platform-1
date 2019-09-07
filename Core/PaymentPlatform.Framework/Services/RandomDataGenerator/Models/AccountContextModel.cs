using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель личного кабинета пользователя для Context.
    /// </summary>
    [Table("Accounts")]
    public class AccountContextModel : AccountModel
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public new Guid Id { get; set; }

        // Навигационные свойства.
        public ProfileContextModel Profile { get; set; }
    }
}
