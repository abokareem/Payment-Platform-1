using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.ViewModels
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
