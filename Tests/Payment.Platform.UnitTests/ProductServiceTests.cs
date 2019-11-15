using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Implementations;
using PaymentPlatform.Product.API.Services.Interfaces;
using System;
using System.Collections.Generic;
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
        private readonly Mock<IRabbitMQService> _rabbitMQService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public ProductServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

            _rabbitMQService = new Mock<IRabbitMQService>();
            _rabbitMQService.Setup(rmq => rmq.SetListener(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns((true, string.Empty));
        }

        /// <summary>
        /// Формирование настроек для ProductContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки ProductContext.</returns>
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

                guid = context.Products.LastOrDefault().Id.ToString();
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

        /// <summary>
        /// Тест на получение всех товаров пользователя.
        /// </summary>
        [Fact]
        public void GetAllProducts_WhenUserIsNotAdmin_Return_Products()
        {
            // Arrange
            var options = GetContextOptions();
            var profileGuid = Guid.NewGuid();
            var productOne = new ProductModel
            {
                Name = "ProductOne",
                Category = "Category",
                Description = "Desc",
                ProfileId = profileGuid,
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };
            var productTwo = new ProductModel
            {
                Name = "ProductTwo",
                Category = "Category",
                Description = "Desc",
                ProfileId = profileGuid,
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };
            var productThree = new ProductModel
            {
                Name = "ProductThree",
                Category = "Category",
                Description = "Desc",
                ProfileId = profileGuid,
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            List<ProductViewModel> result;

            // Act
            using (var context = new ProductContext(options))
            {
                context.Products.Add(productOne);
                context.Products.Add(productTwo);
                context.Products.Add(productThree);
                context.SaveChanges();

                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetAllProductsAsync(false, profileGuid).GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Тест на получение всех товаров пользователя, если у пользователя нет продуктов.
        /// </summary>
        [Fact]
        public void GetAllProducts_WhenUserIsNotAdmin_Return_Empty()
        {
            // Arrange
            var options = GetContextOptions();
            var profileGuid = Guid.NewGuid();

            List<ProductViewModel> result;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetAllProductsAsync(false, profileGuid).GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест на получение всех товаров пользователя, если пользователь администратор.
        /// </summary>
        [Fact]
        public void GetAllProducts_WhenUserIsAdmin_Return_Products()
        {
            // Arrange
            var options = GetContextOptions();
            var profileGuid = Guid.NewGuid();
            var productOne = new ProductModel
            {
                Name = "ProductOne",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };
            var productTwo = new ProductModel
            {
                Name = "ProductTwo",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };
            var productThree = new ProductModel
            {
                Name = "ProductThree",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            List<ProductViewModel> result;

            // Act
            using (var context = new ProductContext(options))
            {
                context.Products.Add(productOne);
                context.Products.Add(productTwo);
                context.Products.Add(productThree);
                context.SaveChanges();

                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetAllProductsAsync(true, profileGuid).GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Тест на получение всех товаров пользователя.
        /// </summary>
        [Fact]
        public void GetProductsByUserId_Return_Products()
        {
            // Arrange
            var options = GetContextOptions();
            var profileId = Guid.NewGuid();
            var productOne = new ProductModel
            {
                Name = "ProductOne",
                Category = "Category",
                Description = "Desc",
                ProfileId = profileId,
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };
            var productTwo = new ProductModel
            {
                Name = "ProductTwo",
                Category = "Category",
                Description = "Desc",
                ProfileId = profileId,
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            List<ProductViewModel> result;

            // Act
            using (var context = new ProductContext(options))
            {
                context.Products.Add(productOne);
                context.Products.Add(productTwo);
                context.SaveChanges();

                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetProductsByUserIdAsync(profileId).GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Тест на получение всех товаров пользователя, если у пользователя нет товаров.
        /// </summary>
        [Fact]
        public void GetProductsByUserId_Return_Empty()
        {
            // Arrange
            var options = GetContextOptions();
            var profileId = Guid.NewGuid();

            List<ProductViewModel> result;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.GetProductsByUserIdAsync(profileId).GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест на обновление данных товара.
        /// </summary>
        [Fact]
        public void UpdateProduct_Return_True()
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

            var result = false;
            ProductModel baseProduct;
            ProductModel updatedProduct;

            // Act
            using (var context = new ProductContext(options))
            {
                context.Products.Add(product);
                context.SaveChanges();

                baseProduct = context.Products.AsNoTracking().LastOrDefault();

                var productFromContext = context.Products.LastOrDefault();
                productFromContext.Name = "NewProduct";
                productFromContext.Category = "NewCategory";
                productFromContext.Description = "NewDesc";
                productFromContext.ProfileId = Guid.NewGuid();
                productFromContext.MeasureUnit = "NewUnit";
                productFromContext.Price = 2;
                productFromContext.Amount = 2;
                productFromContext.IsActive = false;

                var updatedProductViewModel = _mapper.Map<ProductViewModel>(productFromContext);

                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.UpdateProductAsync(updatedProductViewModel).GetAwaiter().GetResult();

                updatedProduct = context.Products.LastOrDefault();
            }

            // Assert
            Assert.True(result);
            Assert.NotEqual(baseProduct.Name, updatedProduct.Name);
            Assert.NotEqual(baseProduct.Category, updatedProduct.Category);
            Assert.NotEqual(baseProduct.Description, updatedProduct.Description);
            Assert.NotEqual(baseProduct.ProfileId, updatedProduct.ProfileId);
            Assert.NotEqual(baseProduct.MeasureUnit, updatedProduct.MeasureUnit);
            Assert.NotEqual(baseProduct.Price, updatedProduct.Price);
            Assert.NotEqual(baseProduct.Amount, updatedProduct.Amount);
            Assert.NotEqual(baseProduct.IsActive, updatedProduct.IsActive);
        }

        /// <summary>
        /// Тест на обновление данных товара, если указанный Id не найден.
        /// </summary>
        [Fact]
        public void UpdateProduct_Return_False()
        {
            // Arrange
            var options = GetContextOptions();
            var product = new ProductViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Product",
                Category = "Category",
                Description = "Desc",
                ProfileId = Guid.NewGuid(),
                MeasureUnit = "Unit",
                Price = 1,
                Amount = 1,
                IsActive = true
            };

            var result = false;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, _rabbitMQService.Object);
                result = productService.UpdateProductAsync(product).GetAwaiter().GetResult();
            }

            // Assert
            Assert.False(result);
        }
    }
}