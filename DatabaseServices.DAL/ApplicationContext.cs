﻿using DatabaseServices.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL
{
	public class ApplicationContext:DbContext
	{
		public DbSet<Transaction> Transactions { get; set; }
		public DbSet<Seller> Sellers { get; set; }

		public ApplicationContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Transaction
			modelBuilder.Entity<Transaction>().Property(p => p.TransactionTime).IsRequired().HasDefaultValueSql("GETDATE()");
			modelBuilder.Entity<Transaction>().Property(p => p.UniqueHashNumber).IsRequired();

			modelBuilder.Entity<Transaction>().HasOne(t => t.Seller).WithMany(s => s.Transactions).HasForeignKey(p=>p.SellerId);
			#endregion
			#region Customer
			modelBuilder.Entity<Customer>().Property(p => p.FirstName).IsRequired();
			modelBuilder.Entity<Customer>().Property(p => p.MiddleName).IsRequired();
			modelBuilder.Entity<Customer>().Property(p => p.LastName).IsRequired();
			modelBuilder.Entity<Customer>().Property(p => p.Email).IsRequired();
			#endregion

			#region Product
			modelBuilder.Entity<Product>().Property(p => p.ProductName).IsRequired();
			modelBuilder.Entity<Product>().Property(p => p.Description).IsRequired();
			modelBuilder.Entity<Product>().Property(p => p.MeasureUnit).IsRequired();
			modelBuilder.Entity<Product>().Property(p => p.Category).IsRequired();
			modelBuilder.Entity<Product>().Property(p => p.QrCode).IsRequired();
			#endregion

			#region Buyer
			modelBuilder.Entity<Buyer>().Property(p => p.Billing).IsRequired();
			#endregion

			#region Seller
			modelBuilder.Entity<Seller>().Property(p => p.OrganisationName).IsRequired();
			modelBuilder.Entity<Seller>().Property(p => p.OrganisationNumber).IsRequired();
			modelBuilder.Entity<Seller>().Property(p => p.ResponsiblePerson).IsRequired();
			modelBuilder.Entity<Seller>().Property(p => p.Billing).IsRequired();
			#endregion
		}
	}
}
