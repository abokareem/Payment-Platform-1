namespace PaymentPlatform.Identity.API.ViewModels
{
    /// <summary>
    /// Учетная запись пользователя в виде ViewModel.
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// Электронная почта.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}
