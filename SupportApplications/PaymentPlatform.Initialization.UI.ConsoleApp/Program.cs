using PaymentPlatform.Initialization.BLL.Implementations;
using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using PaymentPlatform.Initialization.DAL.Models;
using System;

namespace PaymentPlatform.Initialization.UI.ConsoleApp
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var db = new ApplicationContext())
            {
                // создаем два объекта User
                Account user1 = new Account
                {
                    Email = "123",
                    Password = "123",
                    Login = "123",
                    Role = 1,
                    IsActive = true
                };

                // добавляем их в бд
                db.Accounts.Add(user1);
                db.SaveChanges();
            }

            Console.WriteLine("Сервис для генерирования БД и тестовых данных для нее");
            Console.ReadLine();
            //Selector();
        }

        /// <summary>
        /// Обеспечивает консольное управление проектом
        /// </summary>
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
                        operationResult = creationResult == true ? "успешно. Была создана БД." : "с ошибкой.";
                        Console.WriteLine($"Работа завершена {operationResult}");
                        break;
                    case ConsoleKey.D:
                        var deletionResult = dbController.DeleteDatabaseAsync().Result;
                        operationResult = deletionResult == true ? "успешно. Была удалена БД." : "с ошибкой.";
                        Console.WriteLine($"Работа завершена {operationResult}");
                        break;
                    case ConsoleKey.R:
                        var randomiseResult = dbController.AddRandomDataToDatabaseAsync(randomDataGenerator).Result;
                        operationResult = randomiseResult == true ? "успешно. В существеющую БД были добавлены случайные значения." : "с ошибкой.";
                        Console.WriteLine($"Работа завершена {operationResult}");
                        break;
                    case ConsoleKey.E:
                        Console.WriteLine("Завершение работы программы.");
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Команда не определена!");
                        break;
                }
            }
        }
    }
}
