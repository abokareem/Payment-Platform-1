using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

// TODO: Изменить директиву

namespace PaymentPlatform.Product.API.Models
{
	public class ProductContext : DbContext
	{
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<ProductReservedModel> ProductReserves { get; set; }

		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
	}
}
