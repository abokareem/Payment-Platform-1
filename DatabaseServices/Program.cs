using DatabaseServices.BLL.Implementations;
using DatabaseServices.BLL.Interfaces;
using DatabaseServices.DAL;
using System;

namespace DatabaseServices.View
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var context = new ApplicationContext();
			IRandomDataGenerator randomDataGenerator = new RandomDataGenerator(context);

			var dbController = new DatabaseController(context);
			_ = dbController.CreateDatabaseAsync();
			_ = dbController.AddRandomDataToDatabaseAsync(randomDataGenerator);
		}
	}
}
