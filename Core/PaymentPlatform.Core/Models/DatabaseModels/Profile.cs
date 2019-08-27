using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentPlatform.Core.Models.DatabaseModels
{
    /// <summary>
	/// Модель профиля пользователя.
	/// </summary>
	public class Profile
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
		/// Имя.
		/// </summary>
		public string FirstName { get; set; }

        /// <summary>
        /// Фамилия.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество.
        /// </summary>
        public string SecondName { get; set; }

        /// <summary>
        /// Идентификатор продавца.
        /// </summary>
        public bool IsSeller { get; set; }

        /// <summary>
        /// Название организации.
        /// </summary>
        public string OrgName { get; set; }

        /// <summary>
        /// Номер организации (УИН, ИНН).
        /// </summary>
        public string OrgNumber { get; set; }

        /// <summary>
        /// Лицевой счет.
        /// </summary>
        public string BankBook { get; set; }

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal Balance { get; set; }
    }
}
