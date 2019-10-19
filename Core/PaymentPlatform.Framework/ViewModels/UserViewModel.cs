using PaymentPlatform.Framework.Interfaces;
using System;

namespace PaymentPlatform.Framework.ViewModels
{
    /// <summary>
    /// ViewModel для пользователя.
    /// </summary>
    public class UserViewModel : IHasGuidIdentity
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
