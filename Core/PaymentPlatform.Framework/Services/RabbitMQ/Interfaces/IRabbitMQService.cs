using System;

namespace PaymentPlatform.Framework.Services.RabbitMQ.Interfaces
{
    // TODO: XML комментарии
    public interface IRabbitMQService
    {
        (bool success, string message) SendMessage(string message, string recipient);
        (bool success, string message) CheckConnection();
        (bool success, string message) ConfigureService(string host, int port, string virtualHost, string userName, string userPassword);
        (bool success, string message) ConfigureServiceDefault();
        (bool success, string message) SetListener(string channelToListen, Action<string> onIncomingMessage);

    }
}
