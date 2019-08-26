using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Core.Interfaces
{
	public interface IRabbitService
	{
		(bool success, string message) SendMessage(string message, string recipient);
		(bool success, string message) CheckConnection();
		(bool success, string message) ConfigureService(string host, int port, string virtualHost, string userName, string userPassword);
		(bool success, string message) ConfigureServiceDefault();
		(bool success, string message) SetListener(string channelToListen, Action<string> onIncomingMessage);

	}
}
