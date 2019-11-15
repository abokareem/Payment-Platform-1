namespace PaymentPlatform.Framework.Helpers
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Секретный ключ.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Окружение приложения.
        /// false - Windows
        /// true - Docker
        /// </summary>
        public bool IsProduction { get; set; }
    }
}