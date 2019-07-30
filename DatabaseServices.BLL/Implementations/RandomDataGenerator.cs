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
		private readonly ApplicationContext applicationContext;

		public RandomDataGenerator(ApplicationContext applicationContext)
		{
			this.applicationContext = applicationContext;
		}

		public Task<bool> GenerateRandomData()
		{
			throw new NotImplementedException();
		}
	}
}
