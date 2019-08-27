using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Transaction.API.Models
{
    public class TransactionContext : DbContext
    {
        public DbSet<Core.Models.DatabaseModels.Transaction> Transactions { get; set; }

		public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
    }
}
