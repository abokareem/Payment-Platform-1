using DatabaseServices.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Interfaces
{
	public interface IRandomDataGenerator
	{
		Task<bool> GenerateRandomDataAsync();
	}
}
