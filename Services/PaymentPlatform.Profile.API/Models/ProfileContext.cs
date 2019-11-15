using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Profile.API.Models
{
    /// <summary>
    /// Контекст профиля.
    /// </summary>
    public class ProfileContext : DbContext
    {
        /// <summary>
        /// Профили.
        /// </summary>
        public DbSet<ProfileModel> Profiles { get; set; }

        /// <summary>
        /// Резервирование баланса.
        /// </summary>
        public DbSet<BalanceReservedModel> BalanceReserveds { get; set; }

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="options">параметры.</param>
        public ProfileContext(DbContextOptions<ProfileContext> options) : base(options) { }
    }
}