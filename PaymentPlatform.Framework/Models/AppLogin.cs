namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Учетная запись пользователя.
    /// </summary>
    public class AppLogin
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
