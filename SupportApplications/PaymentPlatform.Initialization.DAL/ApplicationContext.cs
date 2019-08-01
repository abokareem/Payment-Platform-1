﻿using PaymentPlatform.Initialization.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Initialization.DAL
{
	/// <summary>
	/// Основной контекст приложения.
	/// </summary>
	public class ApplicationContext : DbContext
	{
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Конфигурация контекста.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true");
		}

		/// <summary>
		/// Реализация FluentAPI.
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Account table

			modelBuilder.Entity<Account>()
				.Property(p => p.Id)
				.HasDefaultValueSql("newsequentialid()")
				.IsRequired();

			modelBuilder.Entity<Account>()
				.Property(p => p.Email)
				.IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.Password)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.Login)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.Role)
                .IsRequired();

            modelBuilder.Entity<Account>()
                .Property(p => p.IsActive)
                .IsRequired();

            modelBuilder.Entity<Account>()
				.HasOne(p => p.Profile)
				.WithOne(a => a.Account)
				.HasForeignKey<Profile>(p => p.Id);

            #endregion



            #region Profile table

            modelBuilder.Entity<Profile>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(p => p.FirstName)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(p => p.LastName)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(p => p.SecondName);

            modelBuilder.Entity<Profile>()
                .Property(p => p.IsSeller)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(p => p.OrgName);

            modelBuilder.Entity<Profile>()
                .Property(p => p.OrgNumber);

            modelBuilder.Entity<Profile>()
                .Property(p => p.BankBook)
                .IsRequired();

            modelBuilder.Entity<Profile>()
                .Property(p => p.Balance)
                .IsRequired();

            #endregion



            #region Product table

            modelBuilder.Entity<Product>()
				.Property(p => p.Id)
				.HasDefaultValueSql("newsequentialid()")
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(p => p.ProfileId)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(p => p.Name)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(p => p.Description)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(p => p.MeasureUnit)
				.IsRequired();

			modelBuilder.Entity<Product>()
				.Property(p => p.Category)
				.IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Amount)
                .IsRequired();

            modelBuilder.Entity<Product>()
				.Property(p => p.Price)
				.IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.QrCode)
                .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.IsActive)
                .IsRequired();

            #endregion



            #region Transaction table

            modelBuilder.Entity<Transaction>()
				.Property(p => p.Id)
				.HasDefaultValueSql("newsequentialid()")
				.IsRequired();

			modelBuilder.Entity<Transaction>()
				.Property(p => p.ProfileId)
				.IsRequired();

			modelBuilder.Entity<Transaction>()
				.Property(p => p.ProductId)
				.IsRequired();

			modelBuilder.Entity<Transaction>()
				.Property(p => p.TransactionTime)
				.IsRequired()
				.HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Transaction>()
                .Property(p => p.Status)
                .IsRequired();

            modelBuilder.Entity<Transaction>()
                .Property(p => p.TotalCost)
                .IsRequired();

            //Foreign Keys
            modelBuilder.Entity<Transaction>()
				.HasOne(t => t.Profile)
				.WithMany(s => s.Transactions)
				.HasForeignKey(p => p.ProfileId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Transaction>()
				.HasOne(t => t.Product)
				.WithMany(s => s.Transactions)
				.HasForeignKey(p => p.ProductId)
				.OnDelete(DeleteBehavior.Restrict);

			#endregion
		}
	}
}
