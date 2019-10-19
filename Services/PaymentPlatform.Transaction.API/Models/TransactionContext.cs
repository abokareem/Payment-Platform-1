using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Transaction.API.Models
{
    /// <summary>
    /// Контекст транзакций.
    /// </summary>
    public class TransactionContext : DbContext
    {
        /// <summary>
        /// Транзакции.
        /// </summary>
        public DbSet<TransactionModel> Transactions { get; set; }

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="options">параметры.</param>
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options) { }
    }
}
