using System;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// ViewModel для пользователя.
    /// </summary>
    public class UserViewModel
    {
        /// <summary>
        /// Идентификатор (GUID).
        /// </summary>
		public Guid Id { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
		public string Role { get; set; }
    }
}
