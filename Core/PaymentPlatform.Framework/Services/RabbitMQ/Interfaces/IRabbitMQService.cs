using System;

namespace PaymentPlatform.Framework.Services.RabbitMQ.Interfaces
{
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
		/// Установить кастомные настройки соединения с RabbitMQ.
		/// </summary>
		/// <param name="host">Хост.</param>
		/// <param name="port">Порт.</param>
		/// <param name="virtualHost">Виртуальный хост. (default / ).</param>
		/// <param name="userName">Имя пользователя.</param>
		/// <param name="userPassword">Пароль.</param>
		/// <returns>(успешность выполнения, сообщение)</returns>
		(bool success, string message) ConfigureService(string host, int port, string virtualHost, string userName, string userPassword);

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
