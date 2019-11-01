using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Context;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
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
        public void AddNewAccountsAndProfilesAsync_Return_10_Entities()
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
    }
}
