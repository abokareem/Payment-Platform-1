using PaymentPlatform.Framework.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель личного кабинета пользователя.
    /// </summary>
    [Table("Account")]
    public class AccountModel : IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Активность.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
