namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Модель пользовательского токена.
    /// </summary>
    public class UserTokenModel
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Роль в приложении
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Токен для аутентификации.
        /// </summary>
        public string Token { get; set; }
    }
}