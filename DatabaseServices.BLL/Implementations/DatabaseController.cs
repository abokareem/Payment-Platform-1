using DatabaseServices.BLL.Interfaces;
using DatabaseServices.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Implementations
{
	public class DatabaseController : IDatabaseController
	{
		private readonly ApplicationContext _context;

		public DatabaseController()
		{
			_context = new ApplicationContext();
		}

		public DatabaseController(ApplicationContext context)
		{
			_context = context;
		}

		public async Task<bool> AddRandomDataToDatabaseAsync(IRandomDataGenerator dataGenerator)
		{
			if (dataGenerator is null)
			{
				throw new ArgumentNullException(nameof(dataGenerator));
			}
			if (await _context.Database.CanConnectAsync())
			{
				try
				{
					var result = await dataGenerator.GenerateRandomDataAsync();
					return result;
				}
				catch (Exception)
				{
					return false;
				}
			}
			return default;
		}


		public async Task<bool> CreateDatabaseAsync()
		{
			var result = await _context.Database.EnsureCreatedAsync();
			return result;
		}

		public async Task<bool> DeleteDatabaseAsync()
		{
			var result = await _context.Database.EnsureDeletedAsync();
			return result;
		}
	}
}
