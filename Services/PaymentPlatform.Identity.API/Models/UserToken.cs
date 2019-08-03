namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Секретный токен.
    /// </summary>
    public class UserToken
    {
        /// <summary>
        /// Токен для авторизации.
        /// </summary>
        public string JWT_Token { get; set; }
    }
}
