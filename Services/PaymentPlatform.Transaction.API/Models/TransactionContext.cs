using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Transaction.API.Models
{
    public class ProfileContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
		public DbSet<ProductReserve> ProductReserves { get; set; }
		public DbSet<BalanceReserve> BalanceReserves { get; set; }

		public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}
