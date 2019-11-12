using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Mapping;
using PaymentPlatform.Framework.Models;
using System.Text;

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

            // Конфигурация AutoMapper
            var autoMapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AccountProfile());
                mc.AddProfile(new ProductProfile());
                mc.AddProfile(new UserProfile());
                mc.AddProfile(new TransactionProfile());
                mc.AddProfile(new BalanceReservedProfile());
                mc.AddProfile(new ProductReservedProfile());
            });
            var mapper = autoMapperConfig.CreateMapper();

            // Конфигурация IOptions<AppSettings>
            var appSettings = new AppSettings() { Secret = "3ce1637ed40041cd94d4853d3e766c4d" };
            var options = Options.Create(appSettings);

            serviceCollection.AddSingleton(options);
            serviceCollection.AddSingleton(mapper);

            // UNDONE: Удалить по итогу (?)
            //serviceCollection.AddDbContext<IdentityContext>(context => context.UseInMemoryDatabase(Guid.NewGuid().ToString()), ServiceLifetime.Transient);
            //serviceCollection.AddScoped<IAccountService, AccountService>();
            //serviceCollection.AddScoped<IProductService, ProductService>();
            //serviceCollection.AddScoped<IProfileService, ProfileService>();
            //serviceCollection.AddScoped<ITransactionService, TransactionService>();
            //serviceCollection.AddScoped<IRandomDataGeneratorService, RandomDataGeneratorService>();
            //serviceCollection.AddSingleton<IRabbitMQService, RabbitMQService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
