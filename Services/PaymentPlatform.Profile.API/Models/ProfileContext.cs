using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Core.Models.DatabaseModels;

namespace PaymentPlatform.Profile.API.Models
{
    public class ProfileContext : DbContext
	{
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<BalanceReserve> BalanceReserves { get; set; }

		public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}
