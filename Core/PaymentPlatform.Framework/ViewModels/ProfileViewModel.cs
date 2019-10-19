using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
	/// ViewModel профиля пользователя.
	/// </summary>
    public class ProfileViewModel : IHasGuidIdentity
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
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
