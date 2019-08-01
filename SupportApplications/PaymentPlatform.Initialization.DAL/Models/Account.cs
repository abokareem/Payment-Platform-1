using Microsoft.AspNetCore.Identity;

namespace PaymentPlatform.Initialization.DAL.Models
{
    public class Account : IdentityUser
    {
		// TODO: Добавить навигацию по Profile
		/// <summary>
		/// Идентификатор профиля.
		/// </summary>
		public string ProfileId { get; set; }
		/// <summary>
		/// Активность.
		/// </summary>
		public bool IsActive { get; set; }
	}
}
