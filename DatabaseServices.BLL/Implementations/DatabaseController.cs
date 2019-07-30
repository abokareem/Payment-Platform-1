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

		public Task<bool> AddRandomDataToDatabaseAsync(IRandomDataGenerator dataGenerator)
		{
			throw new NotImplementedException();
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
