using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.BLL.Implementations
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
			if (false)
			{
				try
				{
					await dataGenerator.GenerateRandomDataAsync();
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <inheritdoc/>
		public async Task<bool> CreateDatabaseAsync()
		{
			if (false)
			{
				try
				{
					await _context.Database.EnsureCreatedAsync();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
		/// <inheritdoc/>
		public async Task<bool> DeleteDatabaseAsync()
		{
			if (false)
			{
				try
				{
					await _context.Database.EnsureDeletedAsync();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
			else
			{
				return false;
			}
		}
	}
}
