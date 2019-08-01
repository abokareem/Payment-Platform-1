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
		public DbSet<Seller> Sellers { get; set; }
		public DbSet<Transaction> Transactions { get; set; }

		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public ApplicationContext()
		{

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
		//	#region Transaction table
		//	modelBuilder.Entity<Transaction>()
		//		.Property(p => p.TransactionTime)
		//		.IsRequired()
		//		.HasDefaultValueSql("GETDATE()");
		//	modelBuilder.Entity<Transaction>()
		//		.Property(p => p.UniqueHashNumber)
		//		.IsRequired();
		//	//Foreign Keys
		//	modelBuilder.Entity<Transaction>()
		//		.HasOne(t => t.Seller)
		//		.WithMany(s => s.Transactions)
		//		.HasForeignKey(p => p.SellerId)
		//		.OnDelete(DeleteBehavior.Restrict);

		//	modelBuilder.Entity<Transaction>()
		//		.HasOne(t => t.Buyer)
		//		.WithMany(s => s.Transactions)
		//		.HasForeignKey(p => p.BuyerId)
		//		.OnDelete(DeleteBehavior.Restrict);

		//	modelBuilder.Entity<Transaction>()
		//		.HasOne(t => t.Product)
		//		.WithMany(s => s.Transactions)
		//		.HasForeignKey(p => p.ProductId)
		//		.OnDelete(DeleteBehavior.Restrict);
		//	#endregion

		//	#region Customer table
		//	modelBuilder.Entity<Customer>()
		//		.Property(p => p.FirstName)
		//		.IsRequired();
		//	modelBuilder.Entity<Customer>()
		//		.Property(p => p.LastName)
		//		.IsRequired();
		//	modelBuilder.Entity<Customer>()
		//		.Property(p => p.Email)
		//		.IsRequired();
		//	//Foreign Keys
		//	modelBuilder.Entity<Customer>()
		//		.HasOne(t => t.Seller)
		//		.WithMany(s => s.Customers)
		//		.HasForeignKey(p => p.SellerId)
		//		.OnDelete(DeleteBehavior.Restrict);

		//	modelBuilder.Entity<Customer>()
		//		.HasOne(t => t.Buyer)
		//		.WithMany(s => s.Customers)
		//		.HasForeignKey(p => p.BuyerId)
		//		.OnDelete(DeleteBehavior.Restrict);
		//	#endregion

		//	#region Product table
		//	modelBuilder.Entity<Product>()
		//		.Property(p => p.ProductName)
		//		.IsRequired();
		//	modelBuilder.Entity<Product>()
		//		.Property(p => p.Description)
		//		.IsRequired();
		//	modelBuilder.Entity<Product>()
		//		.Property(p => p.MeasureUnit)
		//		.IsRequired();
		//	modelBuilder.Entity<Product>()
		//		.Property(p => p.Category)
		//		.IsRequired();
		//	modelBuilder.Entity<Product>()
		//		.Property(p => p.QrCode)
		//		.IsRequired();
		//	//Foreign Keys
		//	modelBuilder.Entity<Product>()
		//		.HasOne(t => t.Seller)
		//		.WithMany(s => s.Products)
		//		.HasForeignKey(p => p.SellerId)
		//		.OnDelete(DeleteBehavior.Restrict);
		//	#endregion

		//	#region Buyer table
		//	modelBuilder.Entity<Buyer>()
		//		.Property(p => p.Billing)
		//		.IsRequired();
		//	#endregion

		//	#region Seller table
		//	modelBuilder.Entity<Seller>()
		//		.Property(p => p.OrganisationName)
		//		.IsRequired();
		//	modelBuilder.Entity<Seller>()
		//		.Property(p => p.OrganisationNumber)
		//		.IsRequired();
		//	modelBuilder.Entity<Seller>()
		//		.Property(p => p.ResponsiblePerson)
		//		.IsRequired();
		//	modelBuilder.Entity<Seller>()
		//		.Property(p => p.Billing)
		//		.IsRequired();
		//	#endregion
		}
	}
}
