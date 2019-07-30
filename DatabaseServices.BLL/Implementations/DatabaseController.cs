using DatabaseServices.BLL.Interfaces;
using DatabaseServices.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Implementations
{
	/// <summary>
	/// Контроллер БД
	/// </summary>
	public class DatabaseController : IDatabaseController
	{
		private readonly ApplicationContext _context;
		/// <summary>
		/// Пустой конструктор
		/// </summary>
		public DatabaseController()
		{
			_context = new ApplicationContext();
		}
		/// <summary>
		/// Конструктор, принимающий контекст БД
		/// </summary>
		/// <param name="context">Контекст работы с БД</param>
		public DatabaseController(ApplicationContext context)
		{
			_context = context;
		}
		/// <inheritdoc/>
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

		/// <inheritdoc/>
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
		/// <inheritdoc/>
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
