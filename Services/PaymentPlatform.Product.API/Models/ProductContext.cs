using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Product.API.Models
{
	public class ProductContext : DbContext
	{
		public DbSet<Product> Products { get; set; }
		public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
	}
}
