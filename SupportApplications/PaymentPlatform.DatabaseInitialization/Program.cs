using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PaymentPlatform.DatabaseInitialization
{
    static class Program
    {
        public static async Task Main()
        {
            Console.Title = "RandomDataGenerator Application v1.0";

            IRandomDataGeneratorService rndDataGenerator = new RandomDataGeneratorService();

            Console.Write(DbInitializationConstants.ENTER_COUNT);
            var value = Environment.GetEnvironmentVariable("COUNT") ?? Console.ReadLine();
            int.TryParse(value, out int count);

            if (count > 0)
            {
                var allTime = 0L;

                allTime += await StartFillingDatabase(DataGeneratorMethod.AddNewAccountsAndProfilesAsync, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(DataGeneratorMethod.AddNewProductsAsync, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(DataGeneratorMethod.AddNewTransactionsAsync, rndDataGenerator, count).ConfigureAwait(false);

                Console.WriteLine(DbInitializationConstants.SUCCESSFUL_COMPLETION + allTime.ToString() + DbInitializationConstants.MS);
            }
            else
            {
                Console.WriteLine(DbInitializationConstants.INVALID_COMMAND);
            }

            Console.ReadLine();
        }

		/// <summary>
		/// Заполнить БД случайными значениями.
		/// </summary>
		/// <param name="method">Используемый метод генератора данных.</param>
		/// <param name="rndDataGenerator">Сервис генерации случайных значений.</param>
		/// <param name="count">Количество значений для генерации.</param>
		/// <returns>Затраченное время.</returns>
        private static async Task<long> StartFillingDatabase(DataGeneratorMethod method, IRandomDataGeneratorService rndDataGenerator, int count)
        {
            var watch = Stopwatch.StartNew();

            switch (method)
            {
                case DataGeneratorMethod.AddNewAccountsAndProfilesAsync: { await rndDataGenerator.AddNewAccountsAndProfilesAsync(count); } break;
                case DataGeneratorMethod.AddNewProductsAsync: { await rndDataGenerator.AddNewProductsAsync(count); } break;
                case DataGeneratorMethod.AddNewTransactionsAsync: { await rndDataGenerator.AddNewTransactionsAsync(count); } break;
            }
            watch.Stop();
            Console.WriteLine(DbInitializationConstants.LEAD_TIME + watch.ElapsedMilliseconds.ToString() + DbInitializationConstants.MS);
            return watch.ElapsedMilliseconds;
        }

		/// <summary>
		/// Перечисление методов генератора данных.
		/// </summary>
		private enum DataGeneratorMethod
		{
			AddNewAccountsAndProfilesAsync = 1,
			AddNewProductsAsync = 2,
			AddNewTransactionsAsync = 3
		}
    }
}
