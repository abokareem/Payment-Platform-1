using System;

namespace PaymentPlatform.Framework.Services.RabbitMQ.Interfaces
{
    /// <summary>
    /// Интерфейс брокера сообщений.
    /// </summary>
    public interface IRabbitMQService
    {
        /// <summary>
        /// Отправить сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="recipient">Получатель.</param>
        /// <returns>(успешность выполнения, сообщение)</returns>
        (bool success, string message) SendMessage(string message, string recipient);

        /// <summary>
        /// Проверить соединение с брокером сообщений.
        /// </summary>
        /// <returns>(успешность выполнения, сообщение)</returns>
        (bool success, string message) CheckConnection();

        /// <summary>
        /// Установить настройки соединения по-умолчанию.
        /// </summary>
        /// <returns>(успешность выполнения, сообщение)</returns>
        (bool success, string message) ConfigureServiceDefault();

        /// <summary>
        /// Установить слушатель.
        /// </summary>
        /// <param name="channelToListen">Канал для прослушивания.</param>
        /// <param name="onIncomingMessage">Метод, выполняющийся при получении сообщения.</param>
        /// <returns>(успешность выполнения, сообщение)</returns>
        (bool success, string message) SetListener(string channelToListen, Action<string> onIncomingMessage);
    }
}
