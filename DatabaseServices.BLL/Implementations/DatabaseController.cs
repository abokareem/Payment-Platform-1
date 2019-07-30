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
					await dataGenerator.GenerateRandomDataAsync();
				}
				catch (Exception)
				{
					return false;
				}
			}
			return true;
		}


		public async Task<bool> CreateDatabaseAsync()
		{
			try
			{
				await _context.Database.EnsureCreatedAsync();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public async Task<bool> DeleteDatabaseAsync()
		{
			try
			{
				await _context.Database.EnsureDeletedAsync();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}
	}
}
