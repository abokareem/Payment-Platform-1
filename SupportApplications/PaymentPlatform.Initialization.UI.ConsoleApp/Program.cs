using PaymentPlatform.Initialization.BLL.Implementations;
using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using PaymentPlatform.Initialization.DAL.Models;
using System;
using System.Threading.Tasks;

namespace PaymentPlatform.Initialization.UI.ConsoleApp
{
    static class Program
    {
        static async Task Main()
        {
            IRandomDataGenerator rndDataGenerator = new RandomDataGenerator();

            await rndDataGenerator.AddNewAccountsAndProfilesAsync(10);
            await rndDataGenerator.AddNewProductsAsync(10);
            await rndDataGenerator.AddNewTransactionsAsync(10);

            Console.WriteLine("Сервис для генерирования БД и тестовых данных для нее");
            Console.ReadLine();
        }
    }
}
