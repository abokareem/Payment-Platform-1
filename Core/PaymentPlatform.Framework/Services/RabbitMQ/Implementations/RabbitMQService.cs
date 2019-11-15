using Microsoft.Extensions.Options;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
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
        private readonly AppSettings _appSettings;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="hostName">Имя хоста.</param>
        /// <param name="port">Порт.</param>
        /// <param name="virtualHost">Виртуальный хост.</param>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="password">Пароль.</param>
        public RabbitMQService(string host, int port, string virtualHost, string userName, string userPassword)
        {
            #region Parameters check

            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_HOST, nameof(host));
            }

            if (port <= 0)
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_PORT, nameof(port));
            }

            if (string.IsNullOrEmpty(virtualHost))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_VIRTUAL_HOST, nameof(virtualHost));
            }

            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_USER_NAME, nameof(userName));
            }

            if (string.IsNullOrEmpty(userPassword))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_USER_NAME, nameof(userPassword));
            }

            #endregion Parameters check

            connectionFactory = new ConnectionFactory
            {
                HostName = host,
                Port = port,
                VirtualHost = virtualHost,
                UserName = userName,
                Password = userPassword
            };
        }

        /// <summary>
        /// Стандартный конструктор.
        /// </summary>
        public RabbitMQService(IOptions<AppSettings> appSettings) => _appSettings = appSettings.Value ?? throw new ArgumentException(nameof(appSettings));

        /// <inheritdoc/>
        public (bool success, string message) CheckConnection()
        {
            if (connection.IsOpen)
            {
                return (true, RabbitMQConstants.CONNECTION_ESTABLISHED);
            }
            else
            {
                return (false, RabbitMQConstants.CONNECTION_FAILED);
            }
        }

        /// <inheritdoc/>
        public (bool success, string message) SendMessage(string message, string recipient)
        {
            #region Parameters check

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_MESSAGE, nameof(message));
            }

            if (string.IsNullOrEmpty(recipient))
            {
                throw new ArgumentException(RabbitMQConstants.INCORRECT_RECIPIENT, nameof(recipient));
            }

            #endregion Parameters check

            try
            {
                using (var channel = connectionFactory.CreateConnection().CreateModel())
                {
                    channel.QueueDeclare(recipient, false, false, false, null);

                    var messageBytes = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish("", recipient, null, messageBytes);
                }
                return (true, RabbitMQConstants.MESSAGE_SENT);
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
                connection = CustomCreateConnection();
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

                return (true, RabbitMQConstants.OPERATION_COMPLETED);
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

        private IConnection CustomCreateConnection()
        {
            return connectionFactory.CreateConnection();
        }

        /// <inheritdoc/>
        public (bool success, string message) ConfigureServiceDefault()
        {
            //var jsonConfiguration = new ConfigurationBuilder()
            //       .SetBasePath(Environment.CurrentDirectory)
            //       .AddJsonFile(@"Settings/RabbitMQConfig.json")
            //       .Build();
            //var settings = new RabbitMQConfig();
            //jsonConfiguration.Bind(settings);

            connectionFactory = new ConnectionFactory
            {
                HostName = string.Empty,
                Port = 5672,
                VirtualHost = "/",
                UserName = "admin",
                Password = "admin"
            };

            if (_appSettings.IsProduction)
            {
                connectionFactory.HostName = "rabbit_mq";
            }
            else
            {
                connectionFactory.HostName = "localhost";
            }

            return (true, RabbitMQConstants.CONFIGURATION_INSTALLED);
        }
    }
}