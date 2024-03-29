﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Implementations;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса TransactionService.
    /// </summary>
    public class TransactionServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly Mock<IRabbitMQService> _rabbitMQService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public TransactionServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

            _rabbitMQService = new Mock<IRabbitMQService>();
            _rabbitMQService.Setup(rmq => rmq.SetListener(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns((true, string.Empty));
            _rabbitMQService.Setup(rmq => rmq.SendMessage(It.IsAny<string>(), It.IsAny<string>())).Returns((true, string.Empty));
        }

        /// <summary>
        /// Формирование настроек для TransactionContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки TransactionContext.</returns>
        private DbContextOptions<TransactionContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<TransactionContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

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
                TotalCost = 1,
                IsActive = true
            };

            var result = false;
            var message = string.Empty;
            Guid guid;

            // Act
            using (var context = new TransactionContext(options))
            {
                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                (result, guid, message) = transactionService.AddNewTransactionAsync(transactionViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Тест на получение транзакции по Guid.
        /// </summary>
        [Fact]
        public void GetTransactionById_Return_Transaction()
        {
            // Arrange
            var options = GetContextOptions();
            var transaction = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 1,
                IsActive = true
            };

            TransactionViewModel result;

            // Act
            using (var context = new TransactionContext(options))
            {
                context.Transactions.Add(transaction);
                context.SaveChanges();

                var guid = context.Transactions.LastOrDefault().Id;

                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = transactionService.GetTransactionByIdAsync(guid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Equal(transaction.ProductId, result.ProductId);
            Assert.Equal(transaction.ProfileId, result.ProfileId);
            Assert.Equal(transaction.TransactionTime, result.TransactionTime);
            Assert.Equal(transaction.TotalCost, result.TotalCost);
            Assert.Equal(transaction.IsActive, result.IsActive);
        }

        /// <summary>
        /// Тест на получение транзакции по Guid, если указанный Guid не существует.
        /// </summary>
        [Fact]
        public void GetTransactionById_Return_Null()
        {
            // Arrange
            var options = GetContextOptions();
            var guid = Guid.NewGuid();

            TransactionViewModel result;

            // Act
            using (var context = new TransactionContext(options))
            {
                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = transactionService.GetTransactionByIdAsync(guid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Тест на получение списка транзакций.
        /// </summary>
        [Fact]
        public void GetTransactionsAsync_Return_Transactions()
        {
            // Arrange
            var options = GetContextOptions();
            var transactionOne = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 1,
                IsActive = true
            };
            var transactionTwo = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 2,
                IsActive = true
            };
            var transactionThree = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 3,
                IsActive = true
            };

            List<TransactionViewModel> result;

            // Act
            using (var context = new TransactionContext(options))
            {
                context.Transactions.Add(transactionOne);
                context.Transactions.Add(transactionTwo);
                context.Transactions.Add(transactionThree);
                context.SaveChanges();

                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = (transactionService.GetTransactionsAsync().GetAwaiter().GetResult()).ToList();
            }

            // Assert
            Assert.Equal(3, result.Count);
        }

        /// <summary>
        /// Тест на получение списка транзакций, если данные отсутствуют.
        /// </summary>
        [Fact]
        public void GetTransactionsAsync_Return_Empty()
        {
            // Arrange
            var options = GetContextOptions();

            List<TransactionViewModel> result;

            // Act
            using (var context = new TransactionContext(options))
            {
                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = (transactionService.GetTransactionsAsync().GetAwaiter().GetResult()).ToList();
            }

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест на отмену транзакции по Id.
        /// </summary>
        [Fact]
        public void RevertTransactionByIdAsync_Return_TrueAndMessage()
        {
            // Arrange
            var options = GetContextOptions();
            var transaction = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 1,
                IsActive = true
            };

            var result = false;
            var message = string.Empty;

            // Act
            using (var context = new TransactionContext(options))
            {
                context.Transactions.Add(transaction);
                context.SaveChanges();

                var guid = context.Transactions.LastOrDefault().Id;

                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                (result, message) = transactionService.RevertTransactionByIdAsync(guid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Тест на обновление транзакции.
        /// </summary>
        [Fact]
        public void UpdateTransaction_Return_True()
        {
            // Arrange
            var options = GetContextOptions();
            var transaction = new TransactionModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 1,
                IsActive = true
            };

            var result = false;
            TransactionModel baseTransaction;
            TransactionModel updatedTransaction;

            // Act
            using (var context = new TransactionContext(options))
            {
                context.Transactions.Add(transaction);
                context.SaveChanges();

                baseTransaction = context.Transactions.AsNoTracking().LastOrDefault();

                var transactionFromContext = context.Transactions.LastOrDefault();
                transactionFromContext.ProductId = Guid.NewGuid();
                transactionFromContext.ProfileId = Guid.NewGuid();
                transactionFromContext.TransactionTime = DateTime.Now.AddDays(1);
                transactionFromContext.TotalCost = 2;
                transactionFromContext.IsActive = false;

                var updatedTransactionViewModel = _mapper.Map<TransactionViewModel>(transactionFromContext);

                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = transactionService.UpdateTransactionAsync(updatedTransactionViewModel).GetAwaiter().GetResult();

                updatedTransaction = context.Transactions.LastOrDefault();
            }

            // Assert
            Assert.True(result);
            Assert.NotEqual(baseTransaction.ProductId, updatedTransaction.ProductId);
            Assert.NotEqual(baseTransaction.ProfileId, updatedTransaction.ProfileId);
            Assert.NotEqual(baseTransaction.TransactionTime, updatedTransaction.TransactionTime);
            Assert.NotEqual(baseTransaction.TotalCost, updatedTransaction.TotalCost);
            Assert.NotEqual(baseTransaction.IsActive, updatedTransaction.IsActive);
        }

        /// <summary>
        /// Тест на обновление транзакции, если указанный Id не найден.
        /// </summary>
        [Fact]
        public void UpdateTransaction_Return_False()
        {
            // Arrange
            var options = GetContextOptions();
            var transactionViewModel = new TransactionViewModel
            {
                ProductId = Guid.NewGuid(),
                ProfileId = Guid.NewGuid(),
                TransactionTime = DateTime.Now,
                TotalCost = 1,
                IsActive = true
            };

            var result = false;

            // Act
            using (var context = new TransactionContext(options))
            {
                ITransactionService transactionService = new TransactionService(context, _mapper, _rabbitMQService.Object);
                result = transactionService.UpdateTransactionAsync(transactionViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.False(result);
        }
    }
}