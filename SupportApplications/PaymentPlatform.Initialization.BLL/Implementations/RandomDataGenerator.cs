using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using PaymentPlatform.Initialization.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.BLL.Implementations
{
	/// <summary>
	/// Генератор случайных данных для заполнения БД
	/// </summary>
	public class RandomDataGenerator : IRandomDataGenerator
	{
		private readonly ApplicationContext _applicationContext;

		/// <summary>
		/// Конструктор, принимающий контекст БД.
		/// </summary>
		/// <param name="applicationContext">Контекст работы с БД</param>
		public RandomDataGenerator(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}

		/// <summary>
		/// Пустой конструктор.
		/// </summary>
		public RandomDataGenerator()
		{
			_applicationContext = new ApplicationContext();
		}

		/// <inheritdoc/>
		public async Task<bool> GenerateRandomDataAsync()
		{
			await Task.Run(() =>
			{
				AddNewAccounts();
				AddNewProfiles();
				AddNewProducts();
				AddNewTransactions();
			});

			return true;
		}

        /// <summary>
        /// Добавляет в БД случайные аккаунты.
        /// </summary>
        private void AddNewAccounts()
        {
            //var accounts = new List<Account>();
            //for (int i = 0; i < 50; i++)
            //{
            //    accounts.Add(new Account
            //    {
            //        Email = $"{Guid.NewGuid().ToString()}@mail.ru",
            //        UserName = Guid.NewGuid().ToString(),
            //    });
            //}
            //_applicationContext.Accounts.AddRange(accounts);
            //_applicationContext.SaveChanges();
        }

        /// <summary>
        /// Добавляет в БД профили.
        /// </summary>
        private void AddNewProfiles()
		{
			//var existedProfiles = _applicationContext.Profiles.Select(eP => eP.Id).ToList();
			//var accounts = _applicationContext.Accounts
			//	.Except(_applicationContext.Accounts
			//					.Join(existedProfiles, l => l.Id, r => r, (l, r) => l))
			//	.ToList();
			//var profiles = new List<Profile>();
			//foreach (var account in accounts)
			//{
			//	profiles.Add(new Profile
			//	{
			//		FirstName = Guid.NewGuid().ToString(),
			//		MiddleName = Guid.NewGuid().ToString(),
			//		LastName = Guid.NewGuid().ToString(),
			//		IsSeller = Convert.ToBoolean(new Random().Next(0, 2)),
			//		OrganisationName = Guid.NewGuid().ToString(),
			//		OrganisationNumber = Guid.NewGuid().ToString(),
			//		BankBook = Guid.NewGuid().ToString(),
			//		Balance = new Random(10).Next(10000),
			//		Account = account,
			//	});
			//}
			//_applicationContext.Profiles.AddRange(profiles);
			//_applicationContext.SaveChanges();
		}

		/// <summary>
		/// Добавляет в БД случайные продукты.
		/// </summary>
		private void AddNewProducts()
		{
			var profiles = _applicationContext.Profiles
				.Where(c => c.IsSeller)
				.ToList();

			var products = new List<Product>();
			int j = 0;
			for (int i = 0; i < 100; i++)
			{
				if (j >= profiles.Count)
				{
					j = 0;
				}
				products.Add(new Product
				{
					ProfileId = profiles[j++].Id,
					Name = Guid.NewGuid().ToString(),
					Description = Guid.NewGuid().ToString(),
					MeasureUnit = Guid.NewGuid().ToString(),
					Category = Guid.NewGuid().ToString(),
					Amount = i * j == 0 ? 1 : i * j,
					Price = i * j * (decimal)Math.PI / 10,
					QrCode = Guid.NewGuid().ToString()
				});
			}
			_applicationContext.Products.AddRange(products);
			_applicationContext.SaveChanges();
		}
		/// <summary>
		/// Добавляет в БД случайные транзакции
		/// </summary>
		private void AddNewTransactions()
		{
			//var transactions = new List<Transaction>();
			//var profiles = _applicationContext.Profiles
			//	.Where(c => !c.IsSeller)
			//	.ToList();

			//var products = _applicationContext.Products
			//	.Where(p => p.Amount > 0)
			//	.Select(x => new { x.Id, x.ProfileId, x.Amount, x.Price })
			//	.ToList();
			//int i = profiles.Count - 1;
			//foreach (var product in products)
			//{
			//	if (i <= 0)
			//	{
			//		i = profiles.Count - 1;
			//	}
			//	var productCount = product.Amount / 2;
			//	transactions.Add(new Transaction
			//	{
			//		ProductId = product.Id,
			//		ProfileId = profiles[i--].Id,
			//		TransactionTime = DateTime.Now,
			//		Status = 0,
			//		ProductCount = productCount,
			//		Total = productCount * product.Price
			//	});
			//}
			//_applicationContext.Transactions.AddRange(transactions);
			//_applicationContext.SaveChanges();
		}
	}
}
