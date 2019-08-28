using PaymentPlatform.Core.Models.DatabaseModels;
using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.BLL.Implementations
{
	/// <summary>
	/// Генератор заполнения базы данных случайными данными.
	/// </summary>
	public class RandomDataGenerator : IRandomDataGenerator
	{
        /// <inheritdoc/>
        public async Task AddNewAccountsAndProfilesAsync(int count)
        {
            using (var db = new ApplicationContext())
            {
                var accounts = new List<Account>();
                var profiles = new List<Profile>();

                for (int i = 0; i < count; i++)
                {
                    accounts.Add(new Account
                    {
                        Email = $"{Guid.NewGuid().ToString()}@outlook.com",
                        Password = Guid.NewGuid().ToString().ToUpper().Substring(0,8),
                        Login = Guid.NewGuid().ToString().ToUpper(),
                        Role = new Random(0).Next(5),
                        IsActive = Convert.ToBoolean(new Random().Next(2))
					});
                }

                await db.Accounts.AddRangeAsync(accounts);

                foreach (var item in accounts)
                {
                    profiles.Add(new Profile
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
            using (var db = new ApplicationContext())
            {
                var rnd = new Random();

                var products = new List<Product>();
                var profilesId = db.Profiles.Where(p=>p.IsSeller).Select(p => p.Id).ToList();

                for (int i = 0; i < count; i++)
                {
                    var index = rnd.Next(profilesId.Count);

                    products.Add(new Product
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
            using (var db = new ApplicationContext())
            {
                var rnd = new Random();

                var transactions = new List<Transaction>();

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

                    transactions.Add(new Transaction
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
