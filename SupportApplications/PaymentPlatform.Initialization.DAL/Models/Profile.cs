using System.Collections.Generic;

namespace PaymentPlatform.Initialization.DAL.Models
{
	/// <summary>
	/// Модель профиля пользователя.
	/// </summary>
	public class Profile
	{
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Фамилия.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Отчество.
		/// </summary>
		public string MiddleName { get; set; }

		/// <summary>
		/// Имя.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Идентификатор продавца.
		/// </summary>
		public bool IsSeller { get; set; }

		/// <summary>
		/// Название организации
		/// </summary>
		public string OrganisationName { get; set; }

		/// <summary>
		/// Номер организации (УИН, ИНН)
		/// </summary>
		public string OrganisationNumber { get; set; }

		/// <summary>
		/// TODO: Что это 
		/// </summary>
		public string BankBook { get; set; }

		/// <summary>
		/// Баланс.
		/// </summary>
		public decimal Balance { get; set; }

		public Account Account { get; set; }
		public ICollection<Transaction> Transactions { get; set; }
		public ICollection<Product> Products { get; set; }
	}
}
