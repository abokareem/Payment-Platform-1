namespace PaymentPlatform.Framework.Services.RabbitMQ.Settings
{
    /// <summary>
    /// Конфигурация RabbitMq
    /// </summary>
    internal class RabbitMQConfig
    {
        /// <summary>
        /// Хост.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Порт.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Виртуальный хост.
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль.
        /// </summary>
        public string Password { get; set; }
    }
}