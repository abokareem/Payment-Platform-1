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
		public string SellerId { get; set; }

		/// <summary>
		/// TODO: Что это 
		/// </summary>
		public string BankBook { get; set; }

		/// <summary>
		/// Баланс.
		/// </summary>
		public decimal Balance { get; set; }

		// TODO: Добавить связи через ICollection
	}
}
