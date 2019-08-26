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
		Task<(bool success, string message)> ConfigureServiceAsync(string host, int port, string virtualHost, string userName, string userPassword);
		Task<(bool success, string message)> ConfigureServiceDefaultAsync();
		Task<(bool success, string message)> SetListenerAsync(string channelToListen, Action<string> onIncomingMessage);

	}
}
