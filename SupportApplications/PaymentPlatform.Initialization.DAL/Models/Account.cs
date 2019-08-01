using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Initialization.DAL.Models
{
    public class Account : IdentityUser
    {
		// TODO: Добавить навигацию по Profile
		/// <summary>
		/// Идентификатор (GUID).
		/// </summary>
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		new public Guid Id { get; set; }
		/// <summary>
		/// Активность.
		/// </summary>
		/// TODO: Использовать активацию в Identity вместо этого? 
		public bool IsActive { get; set; }

		public Profile Profile { get; set; }
	}
}
