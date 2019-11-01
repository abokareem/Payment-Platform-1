using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Context;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Models;
using System;
using System.Linq;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса AccountService.
    /// </summary>
    public class RandomDataGeneratorServiceTests
    {
        /// <summary>
        /// Формирование настроек для MainContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки MainContext.</returns>
        private DbContextOptions<MainContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<MainContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        /// <summary>
        /// Тест на корректное заполнение базы данных случайными данными.
        /// </summary>
        [Fact]
        public void AddNewAccountsAndProfiles_Return_10_Entities()
        {
            // Arrange
            var options = GetContextOptions();
            var countOfEntities = 10;

            var countOfAccounts = 0;
            var countOfProfiles = 0;

            // Act
            using (var context = new MainContext(options))
            {
                IRandomDataGeneratorService randomDataGeneratorService = new RandomDataGeneratorService(context);
                randomDataGeneratorService.AddNewAccountsAndProfilesAsync(countOfEntities).GetAwaiter().GetResult();

                countOfAccounts = context.Accounts.ToList().Count;
                countOfProfiles = context.Profiles.ToList().Count;
            }

            // Assert
            Assert.Equal(countOfEntities, countOfAccounts);
            Assert.Equal(countOfEntities, countOfProfiles);
        }

        /// <summary>
        /// Тест на корректное заполнение базы данных случайными данными.
        /// </summary>
        [Fact]
        public void AddNewProducts_Return_10_Entities()
        {
            // Arrange
            var options = GetContextOptions();

            var profile = new ProfileContextModel
            {
                FirstName = Guid.NewGuid().ToString().Substring(0, 8),
                LastName = Guid.NewGuid().ToString().Substring(0, 8),
                SecondName = Guid.NewGuid().ToString().Substring(0, 8),
                IsSeller = true,
                OrgName = Guid.NewGuid().ToString().Substring(0, 8),
                OrgNumber = Guid.NewGuid().ToString().Substring(0, 8),
                BankBook = Guid.NewGuid().ToString().ToUpper(),
                Balance = new Random(10).Next(10000)
            };

            var countOfEntities = 10;
            var countOfProducts = 0;

            // Act
            using (var context = new MainContext(options))
            {
                context.Profiles.Add(profile);
                context.SaveChanges();

                IRandomDataGeneratorService randomDataGeneratorService = new RandomDataGeneratorService(context);
                randomDataGeneratorService.AddNewProductsAsync(countOfEntities).GetAwaiter().GetResult();

                countOfProducts = context.Products.ToList().Count;
            }

            // Assert
            Assert.Equal(countOfEntities, countOfProducts);
        }

        /// <summary>
        /// Тест на корректное заполнение базы данных случайными данными.
        /// </summary>
        [Fact]
        public void AddNewProducts_WhenNoOneIsSeller_Return_10_Entities()
        {
            // Arrange
            var options = GetContextOptions();

            var countOfEntities = 10;
            var countOfProducts = 0;

            // Act
            using (var context = new MainContext(options))
            {
                IRandomDataGeneratorService randomDataGeneratorService = new RandomDataGeneratorService(context);
                randomDataGeneratorService.AddNewProductsAsync(countOfEntities).GetAwaiter().GetResult();

                countOfProducts = context.Products.ToList().Count;
            }

            // Assert
            Assert.Equal(countOfEntities, countOfProducts);
        }
    }
}
