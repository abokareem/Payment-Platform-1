using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Identity.API.Models
{
    public class IdentityContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }
    }
}
