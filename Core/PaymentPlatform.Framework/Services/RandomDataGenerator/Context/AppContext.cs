using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaymentPlatform.Framework.Models;
using System;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Context
{
    /// <summary>
	/// Основной контекст приложения.
	/// </summary>
    public class MainContext : DbContext
    {
        public DbSet<AccountContextModel> Accounts { get; set; }
        public DbSet<BalanceReservedContextModel> BalanceReserveds { get; set; }
        public DbSet<ProductContextModel> Products { get; set; }
        public DbSet<ProductReservedContextModel> ProductReserveds { get; set; }
        public DbSet<ProfileContextModel> Profiles { get; set; }
        public DbSet<TransactionContextModel> Transactions { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MainContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Конфигурация контекста.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration =
                new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        /// <summary>
		/// Реализация FluentAPI.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Account table

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Password)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Login)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.Role)
                .IsRequired();

            modelBuilder.Entity<AccountContextModel>()
                .Property(p => p.IsActive)
                .IsRequired();


            modelBuilder.Entity<AccountContextModel>()
                .HasOne(p => p.Profile)
                .WithOne(a => a.Account)
                .HasForeignKey<ProfileContextModel>(p => p.Id);

            #endregion

            #region BalanceReserve table
            modelBuilder.Entity<BalanceReservedContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();
            #endregion

            #region ProductReserve table
            modelBuilder.Entity<ProductReservedContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();
            #endregion

            #region Profile table

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.FirstName)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.LastName)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.SecondName);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.IsSeller)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.OrgName);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.OrgNumber);

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.BankBook)
                .IsRequired();

            modelBuilder.Entity<ProfileContextModel>()
                .Property(p => p.Balance)
                .IsRequired();

            #endregion



            #region Product table

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.ProfileId)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Name)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Description)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.MeasureUnit)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Category)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Amount)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.QrCode)
                .IsRequired();

            modelBuilder.Entity<ProductContextModel>()
                .Property(p => p.IsActive)
                .IsRequired();

            #endregion



            #region Transaction table

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.Id)
                .HasDefaultValueSql("newsequentialid()")
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.ProfileId)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.ProductId)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.TransactionTime)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.Status)
                .IsRequired();

            modelBuilder.Entity<TransactionContextModel>()
                .Property(p => p.TotalCost)
                .IsRequired();


            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.Profile)
                .WithMany(s => s.Transactions)
                .HasForeignKey(p => p.ProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.Product)
                .WithMany(s => s.Transactions)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(t => t.BalanceReserved)
                .WithOne(s => s.Transaction)
                .HasForeignKey<BalanceReservedContextModel>(p => p.Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TransactionContextModel>()
                .HasOne(pr => pr.ProductReserved)
                .WithOne(t => t.Transaction)
                .HasForeignKey<ProductReservedContextModel>(pr => pr.Id)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion
        }
    }
}
