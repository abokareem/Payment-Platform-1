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
        /// <summary>
        /// Тест на установку настроек соединения по-умолчанию.
        /// </summary>
        [Fact]
        public void ConfigureServiceDefault_Return_TrueAndMessage()
        {
            // Arrange
            var actual = "Конфигурация установлена успешно.";

            // Act
            IRabbitMQService rabbitMQService = new RabbitMQService();
            var (result, message) = rabbitMQService.ConfigureServiceDefault();

            // Assert
            Assert.True(result);
            Assert.Equal(message, actual);
        }
    }
}
