namespace PaymentPlatform.Framework.Constants
{
    public static class RabbitMQConstants
    {
        /// <summary>
        /// Неверное значение Host.
        /// </summary>
        public static readonly string INCORRECT_HOST = "Host не может быть null или пустым.";

        /// <summary>
        /// Неверное значение Port.
        /// </summary>
        public static readonly string INCORRECT_PORT = "Port не может быть меньше либо равен нулю.";

        /// <summary>
        /// Неверное значение Virtual host.
        /// </summary>
        public static readonly string INCORRECT_VIRTUAL_HOST = "Virtual host не может быть null или пустым.";

        /// <summary>
        /// Неверное значение User name.
        /// </summary>
        public static readonly string INCORRECT_USER_NAME = "Имя пользователя не может быть null или пустым.";

        /// <summary>
        /// Неверное значение Password.
        /// </summary>
        public static readonly string INCORRECT_PASSWORD = "Пароль не может быть null или пустым.";

        /// <summary>
        /// Неверное значение Message.
        /// </summary>
        public static readonly string INCORRECT_MESSAGE = "Сообщение не может быть null или пустым.";

        /// <summary>
        /// Неверное значение Recipient.
        /// </summary>
        public static readonly string INCORRECT_RECIPIENT = "Получатель не может быть null или пустым.";

        /// <summary>
        /// Соединение установлено.
        /// </summary>
        public static readonly string CONNECTION_ESTABLISHED = "Соединение установлено.";

        /// <summary>
        /// Соединение не установлено.
        /// </summary>
        public static readonly string CONNECTION_FAILED = "Соединение не установлено.";

        /// <summary>
        /// Сообщение отправлено.
        /// </summary>
        public static readonly string MESSAGE_SENT = "Сообщение отправлено.";

        /// <summary>
        /// Успешно.
        /// </summary>
        public static readonly string OPERATION_COMPLETED = "Успешно.";

        /// <summary>
        /// Конфигурация установлена успешно.
        /// </summary>
        public static readonly string CONFIGURATION_INSTALLED = "Конфигурация установлена успешно.";
    }
}