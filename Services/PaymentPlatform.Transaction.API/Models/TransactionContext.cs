using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Transaction.API.Models
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }
		public DbSet<ProductReserve> ProductReserves { get; set; }
		public DbSet<BalanceReserve> BalanceReserves { get; set; }

		public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
    }
}
