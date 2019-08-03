namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Учетная запись пользователя.
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Псевдоним.
        /// </summary>
        public string Username { get; set; }
    }
}
