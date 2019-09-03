using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// Личный кабинет пользователя в виде ViewModel.
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// Электронная почта.
        /// </summary>
		[Required(ErrorMessage = "E-mail filed is required")]
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        [Required(ErrorMessage = "Password filed is required")]
        public string Password { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        [Required(ErrorMessage = "Login filed is required")]
        public string Login { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        [Required(ErrorMessage = "Role filed is required")]
        public int? Role { get; set; }

        /// <summary>
        /// Активность.
        /// </summary>
        [Required(ErrorMessage = "IsActive filed is required")]
        public bool? IsActive { get; set; }
    }
}
