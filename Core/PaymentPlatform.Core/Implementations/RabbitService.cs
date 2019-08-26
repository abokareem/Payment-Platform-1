using Microsoft.Extensions.Configuration;
using PaymentPlatform.Core.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Core.Implementations
{
	public class RabbitService : IRabbitService
	{
		private ConnectionFactory connectionFactory;
		private IConnection connection;
		private IModel channel;


		public RabbitService(string hostName, int port, string virtualHost, string userName, string password)
		{
			_ = ConfigureServiceAsync(hostName, port, virtualHost, userName, password);
		}

		public RabbitService()
		{
			_ = ConfigureServiceDefaultAsync();
		}
		public async Task<(bool success, string message)> CheckConnectionAsync()
		{
			return await Task.Run(() =>
			{
				if (connection.IsOpen)
				{
					return (true, "Соединение установлено.");
				}
				else
				{
					return (false, "Соединение не установлено.");
				}
			});
		}

		public async Task<(bool success, string message)> ConfigureServiceAsync(string host, int port, string virtualHost, string userName, string userPassword)
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

			return await Task.Run(() =>
			{
				connectionFactory = new ConnectionFactory()
				{
					HostName = host,
					Port = port,
					VirtualHost = virtualHost,
					UserName = userName,
					Password = userPassword

				};
				return (true, "Конфигурация установлена успешно.");
			});
		}

		public async Task<(bool success, string message)> SendMessageAsync(string message, string recipient)
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
				return await Task.Run(() =>
				{
					using (var connection = connectionFactory.CreateConnection())
					using (var channel = connection.CreateModel())
					{
						channel.QueueDeclare(recipient, false, false, false, null);

						var messageBytes = Encoding.UTF8.GetBytes(message);

						channel.BasicPublish("", recipient, null, messageBytes);
					}
					return (true, "Сообщение отправлено успешно.");
				});
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

		public async Task<(bool success, string message)> SetListenerAsync(string channelToListen, Action<string> onIncomingMessage)
		{
			try
			{
				return await Task.Run(()=>
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
				});
			}
			catch (BrokerUnreachableException brokerException)
			{
				return (false, brokerException.Message);
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
				throw;
			}
		}

		public async Task<(bool success, string message)> ConfigureServiceDefaultAsync()
		{
			return await Task.Run(() =>
			{
				if (!File.Exists("RabbitMqConfig"))
				{
					return (false, "Файл конфигурации не найден.");
				}
				var jsonConfiguration = new ConfigurationBuilder()
					   .SetBasePath(Environment.CurrentDirectory)
					   .AddJsonFile("conf.json").Build();

				var host = jsonConfiguration.GetSection("host").Value;
				var port = jsonConfiguration.GetSection("port").Value;
				var virtualHost = jsonConfiguration.GetSection("virtualHost").Value;
				var userName = jsonConfiguration.GetSection("userName").Value;
				var password = jsonConfiguration.GetSection("password").Value;

				connectionFactory = new ConnectionFactory()
				{
					HostName = host,
					Port = int.Parse(port),
					VirtualHost = virtualHost,
					UserName = userName,
					Password = password
				};
				return (true, "Конфигурация установлена успешно.");
			});

		}
	}
}
