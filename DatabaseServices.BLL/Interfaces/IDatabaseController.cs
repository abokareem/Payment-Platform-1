using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Interfaces
{
	public interface IDatabaseController
	{
		Task<bool> CreateDatabaseAsync();
		Task<bool> DeleteDatabaseAsync();
		Task<bool> AddRandomDataToDatabaseAsync(IRandomDataGenerator dataGenerator);
	}
}
