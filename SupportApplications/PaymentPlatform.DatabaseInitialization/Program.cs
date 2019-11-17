using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Enums;
using PaymentPlatform.Framework.Extensions;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Context;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Implementations;
using PaymentPlatform.Framework.Services.RandomDataGenerator.Interfaces;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace PaymentPlatform.DatabaseInitialization
{
    internal static class Program
    {
        public static async Task Main()
        {
            var programTitle = "RandomDataGenerator Application v2.0";
            Console.Title = programTitle;

            try
            {
                Console.WriteLine($"Welcome to {programTitle}!");

                var serviceCollection = new ServiceCollection();
                var connectionString = GetConnectionString(serviceCollection);

                Console.WriteLine($"ConnectionString: {connectionString}.");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentException(nameof(connectionString));
                }

                serviceCollection.AddDbContext<MainContext>(options => options.UseSqlServer(connectionString));
                var serviceProvider = serviceCollection.BuildServiceProvider();

                var mainContext = serviceProvider.GetService<MainContext>();

                IRandomDataGeneratorService rndDataGenerator = new RandomDataGeneratorService(mainContext);

                Console.Write(DbInitializationConstants.ENTER_COUNT);

                var value = Environment.GetEnvironmentVariable("COUNT") ?? Console.ReadLine();
                int.TryParse(value, out int count);

                if (count > 0)
                {
                    var allTime = 0L;

                    allTime += await StartFillingDatabase(DataGeneratorTypes.AddNewAccountsAndProfilesAsync, rndDataGenerator, count);
                    allTime += await StartFillingDatabase(DataGeneratorTypes.AddNewProductsAsync, rndDataGenerator, count);
                    allTime += await StartFillingDatabase(DataGeneratorTypes.AddNewTransactionsAsync, rndDataGenerator, count);

                    Console.WriteLine(DbInitializationConstants.SUCCESSFUL_COMPLETION + allTime.ToString() + DbInitializationConstants.MS);
                }
                else
                {
                    Console.WriteLine(DbInitializationConstants.INVALID_COMMAND);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
        private static async Task<long> StartFillingDatabase(DataGeneratorTypes method, IRandomDataGeneratorService rndDataGenerator, int count)
        {
            var watch = Stopwatch.StartNew();

            switch (method)
            {
                case DataGeneratorTypes.AddNewAccountsAndProfilesAsync: { await rndDataGenerator.AddNewAccountsAndProfilesAsync(count); } break;
                case DataGeneratorTypes.AddNewProductsAsync: { await rndDataGenerator.AddNewProductsAsync(count); } break;
                case DataGeneratorTypes.AddNewTransactionsAsync: { await rndDataGenerator.AddNewTransactionsAsync(count); } break;
                default: break;
            }

            watch.Stop();

            Console.WriteLine(DbInitializationConstants.LEAD_TIME + watch.ElapsedMilliseconds.ToString() + DbInitializationConstants.MS);

            return watch.ElapsedMilliseconds;
        }

        /// <summary>
        /// Получить строку подключения.
        /// </summary>
        /// <returns>Строка подключения.</returns>
        private static string GetConnectionString(ServiceCollection serviceCollection)
        {
            var connectionString = string.Empty;

            if (File.Exists("appsettings.json"))
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = builder.Build();

                var appSettingSection = configuration.GetSection("AppSettings");
                serviceCollection.Configure<AppSettings>(appSettingSection);

                var appSettings = appSettingSection.Get<AppSettings>();
                var isProduction = appSettings.IsProduction;

                var connectionStringValue = isProduction.ToDbConnectionString();
                connectionString = configuration.GetConnectionString(connectionStringValue);
            }

            return connectionString;
        }
    }
}