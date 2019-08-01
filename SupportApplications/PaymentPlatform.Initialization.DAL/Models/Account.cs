using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Initialization.DAL.Models
{
    public class Account
    {
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
        public int Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public int Password { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        public int Login { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Активность.
        /// </summary>
        public bool IsActive { get; set; }



        // Навигационные свойства.
		public Profile Profile { get; set; }
	}
}
