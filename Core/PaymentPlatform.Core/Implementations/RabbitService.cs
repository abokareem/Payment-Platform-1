using PaymentPlatform.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Core.Implementations
{
	public class RabbitService : IRabbitService
	{
		public System.Threading.Tasks.Task<(bool success, string message)> CheckConnectionAsync()
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<(bool success, string message)> ConfigureService(string host, int port, string virtualHost, string userName, string userPassword)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<(bool success, string message)> SendMessageAsync(string message, string recipient)
		{
			throw new NotImplementedException();
		}

		public System.Threading.Tasks.Task<(bool success, string message)> SetListener(Action onIncomingMessage)
		{
			throw new NotImplementedException();
		}
	}
}
