using PaymentPlatform.Framework.Services.RandomDataGenerator.Context;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations
{
    /// <summary>
    /// Генератор заполнения базы данных случайными данными.
    /// </summary>
    public class RandomDataGeneratorService : IRandomDataGeneratorService
    {
        private readonly MainContext _mainContext;
        private readonly Random rnd = new Random();

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="mainContext">Контекст бд.</param>
        public RandomDataGeneratorService(MainContext mainContext) => _mainContext = mainContext ?? throw new ArgumentException(nameof(mainContext));

        /// <inheritdoc/>
        public async Task AddNewAccountsAndProfilesAsync(int count)
        {
            var accounts = new List<AccountContextModel>();
            var profiles = new List<ProfileContextModel>();

            for (int i = 0; i < count; i++)
            {
                accounts.Add(new AccountContextModel
                {
                    Email = $"{Guid.NewGuid().ToString()}@payment-platform.com",
                    Password = Guid.NewGuid().ToString().ToUpper().Substring(0, 8),
                    Login = Guid.NewGuid().ToString().ToUpper(),
                    Role = rnd.Next(3),
                    IsActive = Convert.ToBoolean(rnd.Next(2))
                });
            }

            await _mainContext.Accounts.AddRangeAsync(accounts);

            foreach (var item in accounts)
            {
                profiles.Add(new ProfileContextModel
                {
                    Id = item.Id,
                    FirstName = Guid.NewGuid().ToString().Substring(0, 8),
                    LastName = Guid.NewGuid().ToString().Substring(0, 8),
                    SecondName = Guid.NewGuid().ToString().Substring(0, 8),
                    Passport = Guid.NewGuid().ToString().Substring(0, 8),
                    IsSeller = Convert.ToBoolean(rnd.Next(2)),
                    OrgName = Guid.NewGuid().ToString().Substring(0, 8),
                    OrgNumber = Guid.NewGuid().ToString().Substring(0, 8),
                    BankBook = Guid.NewGuid().ToString().ToUpper(),
                    Balance = rnd.Next(10000)
                });
            }

            await _mainContext.Profiles.AddRangeAsync(profiles);

            await _mainContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddNewProductsAsync(int count)
        {
            var products = new List<ProductContextModel>();
            var profilesId = _mainContext.Profiles.Where(p => p.IsSeller)
                                                  .Select(p => p.Id)
                                                  .ToList();

            if (!profilesId.Any())
            {
                var profile = new ProfileContextModel
                {
                    FirstName = Guid.NewGuid().ToString().Substring(0, 8),
                    LastName = Guid.NewGuid().ToString().Substring(0, 8),
                    SecondName = Guid.NewGuid().ToString().Substring(0, 8),
                    Passport = Guid.NewGuid().ToString().Substring(0, 8),
                    IsSeller = Convert.ToBoolean(rnd.Next(2)),
                    OrgName = Guid.NewGuid().ToString().Substring(0, 8),
                    OrgNumber = Guid.NewGuid().ToString().Substring(0, 8),
                    BankBook = Guid.NewGuid().ToString().ToUpper(),
                    Balance = rnd.Next(10000)
                };

                await _mainContext.Profiles.AddAsync(profile);
                await _mainContext.SaveChangesAsync();

                var profileId = _mainContext.Profiles.FirstOrDefault().Id;
                profilesId.Add(profileId);
            }

            for (int i = 0; i < count; i++)
            {
                var index = rnd.Next(profilesId.Count);

                products.Add(new ProductContextModel
                {
                    ProfileId = profilesId[index],
                    Name = Guid.NewGuid().ToString().Substring(0, 8),
                    Description = Guid.NewGuid().ToString().ToUpper(),
                    MeasureUnit = Guid.NewGuid().ToString().ToUpper().Substring(0, 4),
                    Category = Guid.NewGuid().ToString().Substring(0, 8),
                    Amount = rnd.Next(10000),
                    Price = rnd.Next(10000),
                    QrCode = Guid.NewGuid().ToString().ToUpper(),
                    IsActive = Convert.ToBoolean(rnd.Next(2))
                });
            }

            await _mainContext.Products.AddRangeAsync(products);
            await _mainContext.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task AddNewTransactionsAsync(int count)
        {
            var transactions = new List<TransactionContextModel>();

            var profilesId = _mainContext.Profiles.Where(p => p.IsSeller == true)
                                                  .Select(p => p.Id)
                                                  .ToList();

            if (!profilesId.Any())
            {
                var profile = new ProfileContextModel
                {
                    FirstName = Guid.NewGuid().ToString().Substring(0, 8),
                    LastName = Guid.NewGuid().ToString().Substring(0, 8),
                    SecondName = Guid.NewGuid().ToString().Substring(0, 8),
                    Passport = Guid.NewGuid().ToString().Substring(0, 8),
                    IsSeller = true,
                    OrgName = Guid.NewGuid().ToString().Substring(0, 8),
                    OrgNumber = Guid.NewGuid().ToString().Substring(0, 8),
                    BankBook = Guid.NewGuid().ToString().ToUpper(),
                    Balance = rnd.Next(10000)
                };

                await _mainContext.Profiles.AddAsync(profile);
                await _mainContext.SaveChangesAsync();

                var profileId = _mainContext.Profiles.FirstOrDefault().Id;
                profilesId.Add(profileId);
            }

            var productsId = _mainContext.Products.Where(p => p.IsActive == true)
                                                  .Select(p => p.Id)
                                                  .ToList();

            if (!productsId.Any())
            {
                var profileId = _mainContext.Profiles.Where(p => p.IsSeller == true)
                                                     .Select(p => p.Id)
                                                     .FirstOrDefault();

                var product = new ProductContextModel
                {
                    ProfileId = profileId,
                    Name = Guid.NewGuid().ToString().Substring(0, 8),
                    Description = Guid.NewGuid().ToString().ToUpper(),
                    MeasureUnit = Guid.NewGuid().ToString().ToUpper().Substring(0, 4),
                    Category = Guid.NewGuid().ToString().Substring(0, 8),
                    Amount = rnd.Next(10000),
                    Price = rnd.Next(10000),
                    QrCode = Guid.NewGuid().ToString().ToUpper(),
                    IsActive = true
                };

                await _mainContext.Products.AddAsync(product);
                await _mainContext.SaveChangesAsync();

                var productId = _mainContext.Products.FirstOrDefault().Id;
                productsId.Add(productId);
            }

            for (int i = 0; i < count; i++)
            {
                var profileIndex = rnd.Next(profilesId.Count);
                var productIndex = rnd.Next(productsId.Count);

                var product = _mainContext.Products.FirstOrDefault(p => p.Id == productsId[productIndex]);

                transactions.Add(new TransactionContextModel
                {
                    ProfileId = profilesId[profileIndex],
                    ProductId = product.Id,
                    IsActive = true,
                    TotalCost = product.Price
                });
            }

            await _mainContext.Transactions.AddRangeAsync(transactions);
            await _mainContext.SaveChangesAsync();
        }
    }
}