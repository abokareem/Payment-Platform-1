using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Implementations;
using PaymentPlatform.Product.API.Services.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса ProductService.
    /// </summary>
    public class ProductServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _options;
        private readonly Mock<IRabbitMQService> _rabbitMQService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public ProductServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<AppSettings>>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

            _rabbitMQService = new Mock<IRabbitMQService>();
            _rabbitMQService.Setup(rmq => rmq.SetListener(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns((true, string.Empty));
        }

        /// <summary>
        /// Формирование настроек для MainContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки MainContext.</returns>
        private DbContextOptions<ProductContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<ProductContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        /// <summary>
        /// Тест на добавление нового товара.
        /// </summary>
        [Fact]
        public void AddNewProduct_Return_Id()
        {
            // Arrange
            var options = GetContextOptions();
            var productViewModel = new ProductViewModel
            {
                Name = "Product",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            var guid = string.Empty;
            var result = string.Empty;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.AddNewProductAsync(productViewModel).GetAwaiter().GetResult();

                guid = context.Products.FirstOrDefault().Id.ToString();
            }

            // Assert
            Assert.Equal(guid, result);
        }

        /// <summary>
        /// Тест на получение данных товара по его Guid.
        /// </summary>
        [Fact]
        public void GetProductById_Return_Product()
        {
            // Arrange
            var options = GetContextOptions();
            var product = new ProductModel
            {
                Name = "Product",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            ProductViewModel result;

            // Act
            using (var context = new ProductContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();

                var guid = context.Products.LastOrDefault().Id;

                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetProductByIdAsync(guid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Equal(product.Name, result.Name);
            Assert.Equal(product.Category, result.Category);
            Assert.Equal(product.Description, result.Description);
            Assert.Equal(product.ProfileId, result.ProfileId);
            Assert.Equal(product.MeasureUnit, result.MeasureUnit);
            Assert.Equal(product.Price, result.Price);
            Assert.Equal(product.Amount, result.Amount);
            Assert.Equal(product.IsActive, result.IsActive);
        }

        /// <summary>
        /// Тест на получение товара по его Guid, если указанный Guid не существует.
        /// </summary>
        [Fact]
        public void GetProductById_Return_Null()
        {
            // Arrange
            var options = GetContextOptions();
            var guid = Guid.NewGuid();

            ProductViewModel result;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetProductByIdAsync(guid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Null(result);
        }


    }
}
