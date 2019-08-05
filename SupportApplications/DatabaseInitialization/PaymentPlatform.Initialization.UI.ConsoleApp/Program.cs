using PaymentPlatform.Initialization.BLL.Implementations;
using PaymentPlatform.Initialization.BLL.Interfaces;
using PaymentPlatform.Initialization.DAL;
using System;
using System.Diagnostics;
using System.Threading.Tasks;


namespace PaymentPlatform.Initialization.UI.ConsoleApp
{
    static class Program
    {
        private static async Task Main()
        {
            Console.Title = "RandomDataGenerator Application v1.0";

            IRandomDataGenerator rndDataGenerator = new RandomDataGenerator();

            Console.Write(Constants.ENTER_COUNT);
            var value = Console.ReadLine();
            int.TryParse(value, out int count);

            if (count > 0)
            {
                var allTime = 0L;

                allTime += await StartFillingDatabase(1, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(2, rndDataGenerator, count).ConfigureAwait(false);
                allTime += await StartFillingDatabase(3, rndDataGenerator, count).ConfigureAwait(false);

                Console.WriteLine(Constants.SUCCESSFUL_COMPLETION + allTime.ToString() + Constants.MS);
            }
            else
            {
                Console.WriteLine(Constants.INVALID_COMMAND);
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
            Console.WriteLine(Constants.LEAD_TIME + watch.ElapsedMilliseconds.ToString() + Constants.MS);

            return watch.ElapsedMilliseconds;
        }
    }
}
