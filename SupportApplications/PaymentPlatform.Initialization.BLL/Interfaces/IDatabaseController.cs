using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.BLL.Interfaces
{
	/// <summary>
	/// Интерфейс контроллера БД
	/// </summary>
	public interface IDatabaseController
	{
		/// <summary>
		/// Создать БД
		/// </summary>
		/// <returns>true - в случае успеха</returns>
		Task<bool> CreateDatabaseAsync();
		/// <summary>
		/// Удалить БД
		/// </summary>
		/// <returns>true - в случае успеха</returns>
		Task<bool> DeleteDatabaseAsync();
		/// <summary>
		/// Добавить случайные значения в БД
		/// </summary>
		/// <param name="dataGenerator">Объект класса, реализующего интерфейс IRandomDataGenerator</param>
		/// <returns>true - в случае успеха</returns>
		Task<bool> AddRandomDataToDatabaseAsync(IRandomDataGenerator dataGenerator);
	}
}
