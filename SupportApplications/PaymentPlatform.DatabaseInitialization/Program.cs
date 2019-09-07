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

                allTime += await StartFillingDatabase(1, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(2, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(3, rndDataGenerator, count).ConfigureAwait(false);

                Console.WriteLine(DbInitializationConstants.SUCCESSFUL_COMPLETION + allTime.ToString() + DbInitializationConstants.MS);
            }
            else
            {
                Console.WriteLine(DbInitializationConstants.INVALID_COMMAND);
            }

            Console.ReadLine();
        }

        private static async Task<long> StartFillingDatabase(int param, IRandomDataGeneratorService rndDataGenerator, int count)
        {
            var watch = Stopwatch.StartNew();

            switch (param)
            {
                case 1: { await rndDataGenerator.AddNewAccountsAndProfilesAsync(count); } break;
                case 2: { await rndDataGenerator.AddNewProductsAsync(count); } break;
                case 3: { await rndDataGenerator.AddNewTransactionsAsync(count); } break;
            }
            watch.Stop();
            Console.WriteLine(DbInitializationConstants.LEAD_TIME + watch.ElapsedMilliseconds.ToString() + DbInitializationConstants.MS);
            return watch.ElapsedMilliseconds;
        }
    }
}
