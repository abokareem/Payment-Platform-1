﻿using DatabaseServices.DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseServices.BLL.Interfaces
{
	/// <summary>
	/// Интерфейс генератора случайных данных
	/// </summary>
	public interface IRandomDataGenerator
	{
		/// <summary>
		/// Заполнить БД случайными значениями
		/// </summary>
		/// <returns>true - в случае успеха</returns>
		Task<bool> GenerateRandomDataAsync();
	}
}
