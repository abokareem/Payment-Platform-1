using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Profile.API.Models
{
    public class ProfileContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }

        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}
