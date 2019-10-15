using Microsoft.Extensions.Configuration;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.Services.RabbitMQ.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Text;

namespace PaymentPlatform.Framework.Services.RabbitMQ.Implementations
{
    /// <summary>
    /// Брокер сообщений.
    /// </summary>
    public class RabbitMQService : IRabbitMQService
    {
        private ConnectionFactory connectionFactory;
        private IConnection connection;
        private IModel channel;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="hostName">Имя хоста.</param>
        /// <param name="port">Порт.</param>
        /// <param name="virtualHost">Виртуальный хост.</param>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        public RabbitMQService(string hostName, int port, string virtualHost, string userName, string password)
        {
            _ = ConfigureService(hostName, port, virtualHost, userName, password);
        }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public RabbitMQService()
        {
            _ = ConfigureServiceDefault();
        }

        /// <inheritdoc/>
        public (bool success, string message) CheckConnection()
        {
            if (connection.IsOpen)
            {
                return (true, "Соединение установлено.");
            }
            else
            {
                return (false, "Соединение не установлено.");
            }
        }

        /// <inheritdoc/>
        public (bool success, string message) ConfigureService(string host, int port, string virtualHost, string userName, string userPassword)
        {
            #region Parameters check

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException("Host не может быть null или пустым.", nameof(host));
            }

            if (port <= 0)
            {
                throw new ArgumentException("Port не может быть меньше либо равен нулю.", nameof(port));
            }

            if (string.IsNullOrEmpty(virtualHost))
            {
                throw new ArgumentException("Virtual host не может быть null или пустым.", nameof(virtualHost));
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("Имя пользователя не может быть null или пустым.", nameof(userName));
            }

            if (string.IsNullOrEmpty(userPassword))
            {
                throw new ArgumentException("Пароль не может быть null или пустым.", nameof(userPassword));
            }

            #endregion

            connectionFactory = new ConnectionFactory
            {
                HostName = host,
                Port = port,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = userPassword

            };

            return (true, "Конфигурация установлена успешно.");
        }

        /// <inheritdoc/>
        public (bool success, string message) SendMessage(string message, string recipient)
        {
            #region Parameters check

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Сообщение не может быть null или пустым.", nameof(message));
            }

            if (string.IsNullOrEmpty(recipient))
            {
                throw new ArgumentException("Получатель не может быть null или пустым.", nameof(recipient));
            }

            #endregion

            try
            {
                using (var channel = connectionFactory.CreateConnection().CreateModel())
                {
                    channel.QueueDeclare(recipient, false, false, false, null);

                    var messageBytes = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", recipient, null, messageBytes);
                }
                return (true, "Сообщение отправлено успешно.");
            }
            catch (BrokerUnreachableException brokerException)
            {
                return (false, brokerException.Message);
            }
            catch (Exception exc)
            {
                throw exc;
            }
        }

        /// <inheritdoc/>
        public (bool success, string message) SetListener(string channelToListen, Action<string> onIncomingMessage)
        {
            try
            {
                connection = connectionFactory.CreateConnection();
                channel = connection.CreateModel();

                channel.QueueDeclare(channelToListen, false, false, false, null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    onIncomingMessage.Invoke(message);
                };

                channel.BasicConsume(channelToListen, true, consumer);
                return (true, "Успешно.");
            }
            catch (BrokerUnreachableException brokerException)
            {
                return (false, brokerException.Message);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                throw exc;
            }
        }

        /// <inheritdoc/>
        public (bool success, string message) ConfigureServiceDefault()
        {
            var jsonConfiguration = new ConfigurationBuilder()
                   .SetBasePath(Environment.CurrentDirectory)
                   .AddJsonFile(@"Settings/RabbitMQConfig.json")
                   .Build();
            var settings = new RabbitMQConfig();
            jsonConfiguration.Bind(settings);

            connectionFactory = new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = settings.VirtualHost,
                UserName = settings.UserName,
                Password = settings.Password
            };

            return (true, "Конфигурация установлена успешно.");
        }
    }
}
