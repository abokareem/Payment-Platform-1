using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Core.Interfaces
{
	public interface IRabbitService
	{
		Task<(bool success, string message)> SendMessageAsync(string message, string recipient);
		Task<(bool success, string message)> CheckConnectionAsync();
		Task<(bool success, string message)> ConfigureService(string host, int port, string virtualHost, string userName, string userPassword);
		Task<(bool success, string message)> SetListener(Action onIncomingMessage);
	}
}
