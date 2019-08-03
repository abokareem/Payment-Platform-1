using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Личный кабинет пользователя.
    /// </summary>
    public class Account : Login
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        public string Username { get; set; }

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
