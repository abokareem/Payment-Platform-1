using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Core.Models.DatabaseModels;

namespace PaymentPlatform.Product.API.Models
{
	public class ProductContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductReserve> ProductReserves { get; set; }
		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
	}
}
