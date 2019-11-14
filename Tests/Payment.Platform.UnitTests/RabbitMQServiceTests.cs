using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Services.RabbitMQ.Implementations;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса AccountService.
    /// </summary>
    public class RabbitMQServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IOptions<AppSettings> _options;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public RabbitMQServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<AppSettings>>();
        }

        /// <summary>
        /// Тест на установку настроек соединения по-умолчанию.
        /// </summary>
        [Fact]
        public void ConfigureServiceDefault_Return_TrueAndMessage()
        {
            // Arrange
            var actual = "Конфигурация установлена успешно.";

            // Act
            IRabbitMQService rabbitMQService = new RabbitMQService(_options);
            var (result, message) = rabbitMQService.ConfigureServiceDefault();

            // Assert
            Assert.True(result);
            Assert.Equal(message, actual);
        }
    }
}
