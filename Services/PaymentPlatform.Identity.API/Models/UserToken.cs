namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Пользовательский токен.
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// Токен для аутентификации.
        /// </summary>
        public string JWT_Token { get; set; }
    }
}
