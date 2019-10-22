using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Identity.API.Models
{
    /// <summary>
    /// Контекст для проекта Identity.
    /// </summary>
    public class IdentityContext : DbContext
    {
        /// <summary>
        /// Аккаунты.
        /// </summary>
        public DbSet<AccountModel> Accounts { get; set; }

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="options">параметры.</param>
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) { }
    }
}
