using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Transaction.API.Models
{
    public class TransactionContext : DbContext
    {
        public DbSet<Transaction> Transactions { get; set; }

		public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
    }
}
