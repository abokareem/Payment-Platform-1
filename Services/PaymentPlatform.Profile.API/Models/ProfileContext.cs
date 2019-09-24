using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Profile.API.Models
{
	/// <summary>
	/// Контекст профиля.
	/// </summary>
    public class ProfileContext : DbContext
	{
		/// <summary>
		/// Профили.
		/// </summary>
		public DbSet<ProfileModel> Profiles { get; set; }
		/// <summary>
		/// Резервирования баланса.
		/// </summary>
		public DbSet<BalanceReservedModel> BalanceReserveds { get; set; }

		public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}
