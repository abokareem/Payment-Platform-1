using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель пользователя
	/// </summary>
	public class Customer
	{
		/// <summary>
		/// Идентификатор
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Имя
		/// </summary>
		public string FirstName { get; set; }
		/// <summary>
		/// Отчество
		/// </summary>
		public string MiddleName { get; set; }
		/// <summary>
		/// Фамилия
		/// </summary>
		public string LastName { get; set; }
		/// <summary>
		/// Электронная почта
		/// </summary>
		public string Email { get; set; }
		/// <summary>
		/// Идентификатор покупателя
		/// </summary>
		public int? BuyerId { get; set; }
		/// <summary>
		/// Идентификатор продавца
		/// </summary>
		public int? SellerId { get; set; }
		/// <summary>
		/// Роль
		/// </summary>
		public int Role { get; set; }
		/// <summary>
		/// Активность пользователя
		/// </summary>
		public bool Activity { get; set; }

		public Buyer Buyer { get; set; }
		public Seller Seller { get; set; }
	}
}
