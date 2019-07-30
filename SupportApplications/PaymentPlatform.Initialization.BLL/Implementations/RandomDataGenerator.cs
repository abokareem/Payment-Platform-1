using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using PaymentPlatform.Initialization.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		/// Конструктор, принимающий контекст БД
		/// </summary>
		/// <param name="applicationContext">Контекст работы с БД</param>
		public RandomDataGenerator(ApplicationContext applicationContext)
		{
			_applicationContext = applicationContext;
		}
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public RandomDataGenerator()
		{
			_applicationContext = new ApplicationContext();
		}

		/// <inheritdoc/>
		public async Task<bool> GenerateRandomDataAsync()
		{
			try
			{
				await Task.Run(() =>
				{
					AddNewSellers();
					AddNewBuyers();
					AddNewCustomers();
					AddNewProducts();
					AddNewTransactions();
				});
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
		/// <summary>
		/// Добавляет в БД случайных продавцов
		/// </summary>
		private void AddNewSellers()
		{
			var sellers = new List<Seller>();
			for (int i = 0; i < 10; i++)
			{
				sellers.Add(new Seller
				{
					OrganisationName = Guid.NewGuid().ToString(),
					OrganisationNumber = Guid.NewGuid().ToString(),
					ResponsiblePerson = Guid.NewGuid().ToString(),
					Billing = Guid.NewGuid().ToString(),
					Balance = i + 1 * 1000
				});
			}
			_applicationContext.Sellers.AddRange(sellers);
			_applicationContext.SaveChanges();
		}
		/// <summary>
		/// Добавляет в БД случайных покупателей
		/// </summary>
		private void AddNewBuyers()
		{
			var buyers = new List<Buyer>();
			for (int i = 0; i < 50; i++)
			{
				buyers.Add(new Buyer
				{
					Billing = Guid.NewGuid().ToString(),
					Balance = i + 1 * 1000
				});
			}
			_applicationContext.Buyers.AddRange(buyers);
			_applicationContext.SaveChanges();
		}
		/// <summary>
		/// Добавляет в БД случайных пользователей
		/// </summary>
		private void AddNewCustomers()
		{
			var customers = new List<Customer>();
			var sellers = _applicationContext.Sellers.Select(x => x).ToList();
			var buyers = _applicationContext.Buyers.Select(x => x).ToList();
			//Покупатели
			for (int i = 0; i < 50; i++)
			{
				customers.Add(new Customer()
				{
					FirstName = Guid.NewGuid().ToString(),
					MiddleName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString(),
					Email = Guid.NewGuid().ToString(),
					BuyerId = buyers[i].Id,
					Role = 0,
					Activity = i % 2 == 0 ? true : false
				});
			}
			//Продавцы
			for (int i = 0; i < 10; i++)
			{
				customers.Add(new Customer()
				{
					FirstName = Guid.NewGuid().ToString(),
					MiddleName = Guid.NewGuid().ToString(),
					LastName = Guid.NewGuid().ToString(),
					Email = Guid.NewGuid().ToString(),
					SellerId = sellers[i].Id,
					Role = 0,
					Activity = i % 2 == 0 ? true : false
				});
			}
			_applicationContext.Customers.AddRange(customers);
			_applicationContext.SaveChanges();
		}
		/// <summary>
		/// Добавляет в БД случайные продукты
		/// </summary>
		private void AddNewProducts()
		{
			var customers = _applicationContext.Customers
				.Where(c => c.Activity == true)
				.ToList();
			var sellers = _applicationContext.Sellers
				.Select(x => x)
				.Join(customers, l => l.Id, r => r.SellerId, (l, r) => l)
				.ToList<Seller>();

			var products = new List<Product>();
			int j = 0;
			for (int i = 0; i < 100; i++)
			{
				if (j >= sellers.Count)
				{
					j = 0;
				}
				products.Add(new Product
				{
					SellerId = sellers[j++].Id,
					ProductName = Guid.NewGuid().ToString(),
					Description = Guid.NewGuid().ToString(),
					MeasureUnit = Guid.NewGuid().ToString(),
					Category = Guid.NewGuid().ToString(),
					Amount = i * j,
					Price = i * j * j / 10,
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
			var transactions = new List<Transaction>();
			var customers = _applicationContext.Customers
				.Where(c => c.Activity == true)
				.ToList();

			var products = _applicationContext.Products
				.Select(x => new { x.Id, x.SellerId })
				.ToList();

			var sellers = _applicationContext.Sellers
				.Join(customers, l => l.Id, r => r.SellerId, (l, r) => l)
				.Join(products, l => l.Id, r => r.SellerId, (r, l) => new { SellerId = r.Id, ProductId = l.Id })
				.ToList();

			var buyers = _applicationContext.Buyers
				.Join(customers, l => l.Id, r => r.BuyerId, (l, r) => l)
				.ToList();

			int sellerCounter = 0,
				buyerCounter = 0;
			for (int i = 0; i < 200; i++)
			{
				if (sellerCounter >= sellers.Count)
				{
					sellerCounter = 0;
				}
				if (buyerCounter >= buyers.Count)
				{
					buyerCounter = 0;
				}
				transactions.Add(new Transaction
				{
					UniqueHashNumber = Guid.NewGuid().ToString(),
					SellerId = sellers[sellerCounter].SellerId,
					BuyerId = buyers[buyerCounter].Id,
					ProductId = sellers[sellerCounter].ProductId,
					TransactionTime = DateTime.Now,
					TransactionStatus = 0
				});
				buyerCounter++;
				sellerCounter++;
			}
			_applicationContext.Transactions.AddRange(transactions);
			_applicationContext.SaveChanges();
		}
	}
}
