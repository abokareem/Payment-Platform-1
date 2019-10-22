using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Product.API.Models
{
    /// <summary>
    /// Контекст продукта.
    /// </summary>
    public class ProductContext : DbContext
    {
        /// <summary>
        /// Продукты.
        /// </summary>
        public DbSet<ProductModel> Products { get; set; }

        /// <summary>
        /// Резервирование продукта.
        /// </summary>
        public DbSet<ProductReservedModel> ProductReserves { get; set; }

        /// <summary>
        /// Базовый конструктор.
        /// </summary>
        /// <param name="options">параметры.</param>
        public ProductContext(DbContextOptions<ProductContext> options) : base(options) { }
    }
}
