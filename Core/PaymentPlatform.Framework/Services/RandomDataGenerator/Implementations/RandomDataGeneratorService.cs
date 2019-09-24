﻿using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Context;
using System.Linq;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations
{
    /// <summary>
	/// Генератор заполнения базы данных случайными данными.
	/// </summary>
    public class RandomDataGeneratorService : IRandomDataGeneratorService
    {
        /// <inheritdoc/>
        public async Task AddNewAccountsAndProfilesAsync(int count)
        {
            using (var db = new MainContext())
            {
                var accounts = new List<AccountContextModel>();
                var profiles = new List<ProfileContextModel>();

                for (int i = 0; i < count; i++)
                {
                    accounts.Add(new AccountContextModel
                    {
                        Email = $"{Guid.NewGuid().ToString()}@outlook.com",
                        Password = Guid.NewGuid().ToString().ToUpper().Substring(0, 8),
                        Login = Guid.NewGuid().ToString().ToUpper(),
                        Role = new Random(0).Next(5),
                        IsActive = Convert.ToBoolean(new Random().Next(2))
                    });
                }

                await db.Accounts.AddRangeAsync(accounts);

                foreach (var item in accounts)
                {
                    profiles.Add(new ProfileContextModel
                    {
                        Id = item.Id,
                        FirstName = Guid.NewGuid().ToString().Substring(0, 8),
                        LastName = Guid.NewGuid().ToString().Substring(0, 8),
                        SecondName = Guid.NewGuid().ToString().Substring(0, 8),
                        IsSeller = Convert.ToBoolean(new Random().Next(2)),
                        OrgName = Guid.NewGuid().ToString().Substring(0, 8),
                        OrgNumber = Guid.NewGuid().ToString().Substring(0, 8),
                        BankBook = Guid.NewGuid().ToString().ToUpper(),
                        Balance = new Random(10).Next(10000)
                    });
                }

                await db.Profiles.AddRangeAsync(profiles);

                await db.SaveChangesAsync();

            }
        }

        /// <inheritdoc/>
        public async Task AddNewProductsAsync(int count)
        {
            using (var db = new MainContext())
            {
                var rnd = new Random();

                var products = new List<ProductContextModel>();
                var profilesId = db.Profiles.Where(p => p.IsSeller).Select(p => p.Id).ToList();

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
                        Amount = new Random(0).Next(10000),
                        Price = new Random(10).Next(10000),
                        QrCode = Guid.NewGuid().ToString().ToUpper(),
                        IsActive = Convert.ToBoolean(new Random().Next(2))
                    });
                }

                await db.Products.AddRangeAsync(products);
                await db.SaveChangesAsync();
            }
        }

        /// <inheritdoc/>
        public async Task AddNewTransactionsAsync(int count)
        {
            using (var db = new MainContext())
            {
                var rnd = new Random();

                var transactions = new List<TransactionContextModel>();

                var profilesId = db.Profiles.Where(p => p.IsSeller == true)
                                            .Select(p => p.Id)
                                            .ToList();

                var productsId = db.Products.Where(p => p.IsActive == true)
                                            .Select(p => p.Id)
                                            .ToList();

                for (int i = 0; i < count; i++)
                {
                    var profileIndex = rnd.Next(profilesId.Count);
                    var productIndex = rnd.Next(productsId.Count);

                    var product = db.Products.FirstOrDefault(p => p.Id == productsId[productIndex]);

                    transactions.Add(new TransactionContextModel
                    {
                        ProfileId = profilesId[profileIndex],
                        ProductId = product.Id,
                        Status = new Random(0).Next(5),
                        TotalCost = product.Price
                    });
                }

                await db.Transactions.AddRangeAsync(transactions);
                await db.SaveChangesAsync();
            }
        }
    }
}
