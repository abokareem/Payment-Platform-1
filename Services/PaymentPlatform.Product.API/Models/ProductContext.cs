using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Models
{
	public class ProductContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
	}
}
