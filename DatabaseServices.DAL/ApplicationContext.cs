using DatabaseServices.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseServices.DAL
{
	public class ApplicationContext:DbContext
	{
		public ApplicationContext()
		{
			Database.EnsureCreated();
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true");
		}

		public void OnModelCreating()
		{
			//TODO: Заполнить
		}
	}
}
