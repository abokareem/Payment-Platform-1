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
			var dbDeletionResult = dbController.DeleteDatabaseAsync().Result;
			var dbCreationResult = dbController.CreateDatabaseAsync().Result;
			var randomAdditisionResult = dbController.AddRandomDataToDatabaseAsync(randomDataGenerator).Result;
			Console.WriteLine(dbDeletionResult);
			Console.WriteLine(dbCreationResult);
			Console.WriteLine(randomAdditisionResult);
			Console.ReadLine();
		}
	}
}
