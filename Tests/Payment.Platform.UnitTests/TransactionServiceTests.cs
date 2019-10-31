using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Implementations;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса AccountService.
    /// </summary>
    public class TransactionServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _options;
        private readonly Mock<IRabbitMQService> _rabbitMQService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public TransactionServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<AppSettings>>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

            _rabbitMQService = new Mock<IRabbitMQService>();
            _rabbitMQService.Setup(rmq => rmq.SetListener(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns((true, string.Empty));
            _rabbitMQService.Setup(rmq => rmq.SendMessage(It.IsAny<string>(), It.IsAny<string>())).Returns((true, string.Empty));
        }

        /// <summary>
        /// Формирование настроек для MainContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки MainContext.</returns>
        private DbContextOptions<TransactionContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<TransactionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        // UNDONE: Возможно стоит реализовать функционал иначе (?)
        /// <summary>
        /// Тест на добавление новой транзакции.
        /// </summary>
        [Fact]
        public void AddNewTransaction_Return_TrueAndMessage()
        {
            // Arrange
            var options = GetContextOptions();
            var transactionViewModel = new TransactionViewModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                Amount = 1,
                TotalCost = 1,
                Status = 1
            };

            var result = false;
            var message = string.Empty;

            // Act
            using (var context = new TransactionContext(options))
            {
                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                (result, message) = transactionService.AddNewTransactionAsync(transactionViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.True(result);
        }
    }
}
