using Microsoft.Extensions.DependencyInjection;
using PaymentPlatform.Identity.API.Services.Implementations;
using PaymentPlatform.Identity.API.Services.Interfaces;
using PaymentPlatform.Product.API.Services.Implementations;
using PaymentPlatform.Product.API.Services.Interfaces;
using PaymentPlatform.Profile.API.Services.Implementations;
using PaymentPlatform.Profile.API.Services.Interfaces;
using PaymentPlatform.Transaction.API.Services.Implementations;
using PaymentPlatform.Transaction.API.Services.Interfaces;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Вспомогательный класс для внедрения DI и InMemoryDatabase.
    /// </summary>
    public class ServiceFixture
    {
        /// <summary>
        /// Набор сервисов.
        /// </summary>
        public ServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ServiceFixture()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<IProductService, ProductService>();
            serviceCollection.AddScoped<IProfileService, ProfileService>();
            serviceCollection.AddScoped<ITransactionService, TransactionService>();

            //serviceCollection.AddScoped<IRandomDataGeneratorService, RandomDataGeneratorService>();
            //serviceCollection.AddSingleton<IRabbitMQService, RabbitMQService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
