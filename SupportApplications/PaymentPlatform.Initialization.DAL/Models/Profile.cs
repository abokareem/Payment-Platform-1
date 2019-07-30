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
        /// Имя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
		/// Название организации.
		/// </summary>
		public string OrganizationName { get; set; }

        /// <summary>
        /// Номер организации (УИН, ИНН).
        /// </summary>
        public string OrganizationNumber { get; set; }

        /// <summary>
        /// Ответственное лицо.
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// Счет.
        /// </summary>
        public string Billing { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal Balance { get; set; }

        // TODO: Добавить связи через ICollection
    }
}
