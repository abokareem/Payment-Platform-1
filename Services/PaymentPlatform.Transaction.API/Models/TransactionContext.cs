using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Transaction.API.Models
{
    public class TransactionContext : DbContext
    {
        public DbSet<TransactionModel> Transactions { get; set; }

		public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
    }
}
