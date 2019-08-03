using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Identity.API.Models
{
    public class IdentityContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) {}
    }
}
