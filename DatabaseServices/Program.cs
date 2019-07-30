using DatabaseServices.BLL.Implementations;
using System;

namespace DatabaseServices.View
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");
			var dbController = new DatabaseController();
			_ = dbController.CreateDatabaseAsync();
		}
	}
}
