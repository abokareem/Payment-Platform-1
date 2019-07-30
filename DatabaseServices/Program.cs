﻿using DatabaseServices.BLL.Implementations;
using DatabaseServices.BLL.Interfaces;
using DatabaseServices.DAL;
using System;

namespace DatabaseServices.UI
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Сервис для генерирования БД и тестовых данных для нее");

			Selector();
		}

		static void Selector()
		{
			bool exit = false;

			var context = new ApplicationContext();
			IRandomDataGenerator randomDataGenerator = new RandomDataGenerator(context);
			var dbController = new DatabaseController(context);

			Console.WriteLine("Управление:");
			Console.WriteLine("Нажмите С - для создания БД.");
			Console.WriteLine("Нажмите D - для удаления БД.");
			Console.WriteLine("Нажмите R - для добавления случайных записей в БД (при множественном использовании адекватную работу не гарантирую).");
			Console.WriteLine("Нажмите E - для выхода.");

			while (!exit)
			{
				var key = Console.ReadKey().Key;
				Console.WriteLine();
				string operationResult;
				switch (key)
				{
					case ConsoleKey.C:
						var creationResult = dbController.CreateDatabaseAsync().Result;
						operationResult = creationResult == true ? "успешно" : "с ошибкой";
						Console.WriteLine($"Работа завершена {operationResult}. Была создана БД.");
						break;
					case ConsoleKey.D:
						var deletionResult = dbController.DeleteDatabaseAsync().Result;
						operationResult = deletionResult == true ? "успешно" : "с ошибкой";
						Console.WriteLine($"Работа завершена {operationResult}. Была удалена БД.");
						break;
					case ConsoleKey.R:
						var randomiseResult = dbController.AddRandomDataToDatabaseAsync(randomDataGenerator).Result;
						operationResult = randomiseResult == true ? "успешно" : "с ошибкой";
						Console.WriteLine($"Работа завершена {operationResult}. В существеющую БД были добавлены случайные значения.");
						break;
					case ConsoleKey.E:
						Console.WriteLine("Завершение работы программы.");
						exit = true;
						break;
					default:
						break;
				}
			}
		}
	}
}
