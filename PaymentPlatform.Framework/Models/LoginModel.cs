namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель учетной записи пользователя.
    /// </summary>
    public class LoginModel
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
