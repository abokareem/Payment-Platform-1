using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Profile.API.Models
{
    public class ProfileContext : DbContext
	{
		public DbSet<ProfileModel> Profiles { get; set; }
		public DbSet<BalanceReservedModel> BalanceReserveds { get; set; }

		public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}
