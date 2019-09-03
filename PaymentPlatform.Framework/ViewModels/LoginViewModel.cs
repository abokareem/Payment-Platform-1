using System.ComponentModel.DataAnnotations;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// Учетная запись пользователя в виде ViewModel.
    /// </summary>
    public class LoginViewModel
    {
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
    }
}
