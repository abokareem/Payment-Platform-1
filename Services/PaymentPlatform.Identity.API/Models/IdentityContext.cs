using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Контекст для проекта Identity.
    /// </summary>
    public class IdentityContext : DbContext
    {
        /// <summary>
        /// DbSet.
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="options">параметры.</param>
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) {}
    }
}
