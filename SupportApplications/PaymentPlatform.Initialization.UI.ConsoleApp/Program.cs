using PaymentPlatform.Initialization.BLL.Implementations;
using PaymentPlatform.Initialization.BLL.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace PaymentPlatform.Initialization.UI.ConsoleApp
{
    static class Program
    {
        static async Task Main()
        {
            Console.Title = "RandomDataGenerator Application v1.0";

            IRandomDataGenerator rndDataGenerator = new RandomDataGenerator();

            Console.Write("Пожалуйста, укажите количество для генерации случайных значений: ");
            var value = Console.ReadLine();
            int.TryParse(value, out int count);

            if (count > 0)
            {
                var allTime = 0L;

                allTime += await StartFillingDatabase(1, rndDataGenerator, count);
                allTime += await StartFillingDatabase(2, rndDataGenerator, count);
                allTime += await StartFillingDatabase(3, rndDataGenerator, count);

                Console.WriteLine("\nУспешное завершение программы. Время выполнения: " + allTime.ToString() + " мс.");
            }
            else
            {
                Console.WriteLine("Внимание! Введено неверное значение. Завершение работы программы..");
            }

            Console.ReadLine();
        }

        private static async Task<long> StartFillingDatabase(int param, IRandomDataGenerator rndDataGenerator, int count)
        {
            var watch = Stopwatch.StartNew();

            switch (param)
            {
                case 1: { await rndDataGenerator.AddNewAccountsAndProfilesAsync(count); } break;
                case 2: { await rndDataGenerator.AddNewProductsAsync(count); } break;
                case 3: { await rndDataGenerator.AddNewTransactionsAsync(count); } break;

                default: break;
            }

            watch.Stop();
            Console.WriteLine("Время выполнения метода: " + watch.ElapsedMilliseconds.ToString() + " мс.");

            return watch.ElapsedMilliseconds;
        }
    }
}
