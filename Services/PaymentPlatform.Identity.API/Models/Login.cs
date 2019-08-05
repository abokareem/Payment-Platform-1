namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Учетная запись пользователя.
    /// </summary>
    public class Login
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
