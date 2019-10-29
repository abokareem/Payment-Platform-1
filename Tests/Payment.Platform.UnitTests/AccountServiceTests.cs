using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Implementations;
using PaymentPlatform.Identity.API.Services.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс для тестов класса AccountService.
    /// </summary>
    public class AccountServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IOptions<AppSettings> _options;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public AccountServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _options = _serviceProvider.GetRequiredService<IOptions<AppSettings>>();
            _mapper = _serviceProvider.GetRequiredService<IMapper>();
        }

        /// <summary>
        /// Формирование настроек для MainContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки MainContext.</returns>
        private DbContextOptions<IdentityContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<IdentityContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        /// <summary>
        /// Тест на регистрацию пользователя.
        /// </summary>
        [Fact]
        public void CanUserBeRegistered_Return_True()
        {
            // Arrange
            var options = GetContextOptions();
            var accountViewModel = new AccountViewModel
            {
                Login = "Login",
                Email = "Email@Email.Email",
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            var result = false;
            var message = string.Empty;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                (result, message) = accountService.RegistrationAsync(accountViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.True(result);
            Assert.Equal(IdentityConstants.USER_REGISTRATION_SUCCESS, message);
        }

        /// <summary>
        /// Тест на регистрацию пользователя, если пользователь c таким электронным адресом уже существует.
        /// </summary>
        [Fact]
        public void CanUserBeRegistered_Return_False()
        {
            // Arrange
            var options = GetContextOptions();
            var accountViewModel = new AccountViewModel
            {
                Login = "Login",
                Email = "Email@Email.Email",
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            var result = false;
            var message = string.Empty;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                _ = accountService.RegistrationAsync(accountViewModel).GetAwaiter().GetResult();

                (result, message) = accountService.RegistrationAsync(accountViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.False(result);
            Assert.Equal(IdentityConstants.USER_EXIST, message);
        }

        /// <summary>
        /// Тест на получение данных пользователя по Email.
        /// </summary>
        [Fact]
        public void GetAccountByEmail_Return_Account()
        {
            // Arrange
            var options = GetContextOptions();
            var email = "Email@Email.Email";
            var account = new AccountModel
            {
                Login = "Login",
                Email = email,
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            AccountViewModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                context.Accounts.Add(account);
                context.SaveChanges();

                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAccountByEmailAsync(email).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Equal(account.Login, result.Login);
            Assert.Equal(account.Email, result.Email);
            Assert.Equal(account.Password, result.Password);
            Assert.Equal(account.Role, result.Role);
            Assert.Equal(account.IsActive, result.IsActive);
        }

        /// <summary>
        /// Тест на получение данных пользователя по Email, если указанный Email не существует.
        /// </summary>
        [Fact]
        public void GetAccountByEmail_Return_Null()
        {
            // Arrange
            var options = GetContextOptions();
            var email = "Email@Email.Email";

            AccountViewModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAccountByEmailAsync(email).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// Тест на получение данных пользователя по Id.
        /// </summary>
        [Fact]
        public void GetAccountById_Return_Account()
        {
            // Arrange
            var options = GetContextOptions();
            var email = "Email@Email.Email";
            var account = new AccountModel
            {
                Login = "Login",
                Email = email,
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            AccountViewModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                context.Accounts.Add(account);
                context.SaveChanges();

                var accoundGuid = context.Accounts.LastOrDefault().Id;

                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAccountByIdAsync(accoundGuid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Equal(account.Login, result.Login);
            Assert.Equal(account.Email, result.Email);
            Assert.Equal(account.Password, result.Password);
            Assert.Equal(account.Role, result.Role);
            Assert.Equal(account.IsActive, result.IsActive);
        }

        /// <summary>
        /// Тест на получение данных пользователя по Id, если указанный Id не существует.
        /// </summary>
        [Fact]
        public void GetAccountById_Return_Null()
        {
            // Arrange
            var options = GetContextOptions();
            var accoundGuid = Guid.NewGuid();

            AccountViewModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAccountByIdAsync(accoundGuid).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Null(result);
        }
    }
}
