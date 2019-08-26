using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Core.Interfaces
{
	public interface IRabbitService
	{
		Task<(bool success, string message)> SendMessageAsync(string message, string recipient);
	}
}
