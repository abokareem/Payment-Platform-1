using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Framework.Enums
{
	public enum RabbitMessageActions
	{
		/// <summary>
		/// Применить.
		/// </summary>
		Apply = 1,

		/// <summary>
		/// Отменить.
		/// </summary>
		Revert = 2,
	}
}
