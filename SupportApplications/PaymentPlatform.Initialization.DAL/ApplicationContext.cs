using PaymentPlatform.Initialization.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace PaymentPlatform.Initialization.DAL
{
	/// <summary>
	/// Основной контекст приложения
	/// </summary>
	public class ApplicationContext:DbContext
	{
		public DbSet<Account> Accounts { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<Profile> Profiles { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public ApplicationContext()
		{
			Database.EnsureDeleted();
			Database.EnsureCreated();
		}

		/// <summary>
		/// Конфигурация контекста
		/// </summary>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true");
		}

		/// <summary>
		/// Реализация FluentAPI
		/// </summary>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Account table

			modelBuilder.Entity<Account>()
				.Property(p => p.Id)
				.IsRequired();
			modelBuilder.Entity<Account>()
				.HasOne(p => p.Profile)
				.WithOne(a => a.Account)
				.HasForeignKey<Profile>(p => p.Id);
			#endregion

			#region Product table
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
				.Property(p => p.Price)
				.IsRequired();
			modelBuilder.Entity<Product>()
				.Property(p => p.QrCode)
				.IsRequired();
			//Foreign Keys

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
			//Foreign Keys
			//modelBuilder.Entity<Profile>()
			//	.HasOne(t => t.Account)
			//	.WithOne(s => s.Profile)
			//	.HasForeignKey<Account>(p => p.Id)
			//	.OnDelete(DeleteBehavior.SetNull);
			#endregion

			#region Transaction table
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

			#region Buyer table
			//	modelBuilder.Entity<Buyer>()
			//		.Property(p => p.Billing)
			//		.IsRequired();
			#endregion
		}
	}
}
