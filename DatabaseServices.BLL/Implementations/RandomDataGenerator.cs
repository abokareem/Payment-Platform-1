using DatabaseServices.BLL.Interfaces;
using DatabaseServices.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Implementations
{
	public class RandomDataGenerator : IRandomDataGenerator
	{
		private readonly ApplicationContext _applicationContext;

		public RandomDataGenerator(ApplicationContext applicationContext)
		{
			this._applicationContext = applicationContext;
		}

		public async Task<bool> GenerateRandomDataAsync()
		{
			try
			{
				await Task.Run(() =>
				{
					AddNewCustomers();
					AddNewSellers();
					AddNewBuyers();
					AddNewProducts();
					AddNewTransactions();
				});
			}
			catch (Exception)
			{
				return false;
			}
			return default;
		}

		private void AddNewCustomers()
		{

		}
		private void AddNewSellers()
		{

		}

		private void AddNewBuyers()
		{

		}

		private void AddNewProducts()
		{

		}

		private void AddNewTransactions()
		{

		}
	}
}
