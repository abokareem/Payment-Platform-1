﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Implementations;
using PaymentPlatform.Product.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public ProductServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<AppSettings>>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
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
                IsActive= true
            };

            var guid = string.Empty;
            var result = string.Empty;

            // Act
            using (var context = new ProductContext(options))
            {
                IProductService productService = new ProductService(context, _mapper, null);
                result = productService.AddNewProductAsync(productViewModel).GetAwaiter().GetResult();

                guid = context.Products.FirstOrDefault().Id.ToString();
            }

            // Assert
            Assert.Equal(guid, result);
        }
    }
}
