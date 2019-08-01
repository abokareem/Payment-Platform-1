using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL.Models
{
	public class Seller
	{
		/// <summary>
		/// Идентификатор компании
		/// </summary>
		public string Id { get; set; }
		/// <summary>
		/// Название организации
		/// </summary>
		public string OrganisationName { get; set; }
		/// <summary>
		/// Номер организации (УИН, ИНН)
		/// </summary>
		public string OrganisationNumber { get; set; }
	}
}
