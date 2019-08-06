using System.ComponentModel.DataAnnotations;

namespace PaymentPlatform.Identity.API.ViewModels
{
    /// <summary>
    /// Личный кабинет пользователя в виде ViewModel.
    /// </summary>
    public class AccountViewModel
    {
        /// <summary>
        /// Электронная почта.
        /// </summary>
		[Required]
        public string Email { get; set; }

		/// <summary>
		/// Пароль.
		/// </summary>
		[Required]
		public string Password { get; set; }

		/// <summary>
		/// Псевдоним.
		/// </summary>
		[Required]
		public string Login { get; set; }

		/// <summary>
		/// Роль.
		/// </summary>
		[Required]
		public int? Role { get; set; }

		/// <summary>
		/// Активность.
		/// </summary>
		[Required]
		public bool? IsActive { get; set; }
    }
}
