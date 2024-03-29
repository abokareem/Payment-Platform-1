﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Extensions;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Implementations;
using PaymentPlatform.Identity.API.Services.Interfaces;
using System;
using System.Collections.Generic;
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
        /// Формирование настроек для IdentityContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки IdentityContext.</returns>
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
        /// Тест на аутентификацию пользователя.
        /// </summary>
        [Fact]
        public void CanUserBeAuthenticated_Return_UserTokenModel()
        {
            // Arrange
            var options = GetContextOptions();
            var account = new AccountModel
            {
                Login = "Login",
                Email = "Email@Email.Email",
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };
            var loginViewModel = new LoginViewModel
            {
                Email = "Email@Email.Email",
                Password = "P@ssword"
            };

            UserTokenModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                context.Accounts.Add(account);
                context.SaveChanges();

                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.AuthenticateAsync(loginViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Equal(account.Login, result.UserName);
            Assert.Equal(account.Role.ConvertRole(), result.Role);
        }

        /// <summary>
        /// Тест на аутентификацию пользователя, если Email и(или) пароль не совпадают.
        /// </summary>
        [Fact]
        public void CanUserBeAuthenticated_Return_Null()
        {
            // Arrange
            var options = GetContextOptions();
            var loginViewModel = new LoginViewModel
            {
                Email = "AnotherEmail@Email.Email",
                Password = "AnotherP@ssword"
            };

            UserTokenModel result;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.AuthenticateAsync(loginViewModel).GetAwaiter().GetResult();
            }

            // Assert
            Assert.Null(result);
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

        /// <summary>
        /// Тест на получение всех учетных записей пользователей.
        /// </summary>
        [Fact]
        public void GetAllAccounts_Return_Accounts()
        {
            // Arrange
            var options = GetContextOptions();
            var accountOne = new AccountModel
            {
                Login = "LoginOne",
                Email = "EmailOne@Email.Email",
                Password = "P@sswordOne",
                Role = 1,
                IsActive = true
            };
            var accountTwo = new AccountModel
            {
                Login = "LoginTwo",
                Email = "EmailTwo@Email.Email",
                Password = "P@sswordTwo",
                Role = 1,
                IsActive = true
            };

            List<AccountViewModel> result;

            // Act
            using (var context = new IdentityContext(options))
            {
                context.Accounts.Add(accountOne);
                context.Accounts.Add(accountTwo);
                context.SaveChanges();

                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAllAccountsAsync().GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Equal(2, result.Count);
        }

        /// <summary>
        /// Тест на получение всех учетных записей пользователей, если данные отсутствуют.
        /// </summary>
        [Fact]
        public void GetAllAccounts_Return_Empty()
        {
            // Arrange
            var options = GetContextOptions();

            List<AccountViewModel> result;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.GetAllAccountsAsync().GetAwaiter().GetResult().ToList();
            }

            // Assert
            Assert.Empty(result);
        }

        /// <summary>
        /// Тест на обновление данных пользователя.
        /// </summary>
        [Fact]
        public void UpdateAccount_Return_True()
        {
            // Arrange
            var options = GetContextOptions();
            var account = new AccountModel
            {
                Login = "Login",
                Email = "Email@Email.Email",
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            var result = false;
            AccountModel baseAccount;
            AccountModel updatedAccount;

            // Act
            using (var context = new IdentityContext(options))
            {
                context.Accounts.Add(account);
                context.SaveChanges();

                baseAccount = context.Accounts.AsNoTracking().LastOrDefault();

                var accountFromContext = context.Accounts.LastOrDefault();
                accountFromContext.Login = "NewLogin";
                accountFromContext.Email = "NewEmail@Email.Email";
                accountFromContext.Password = "NewP@ssword";
                accountFromContext.Role = 2;
                accountFromContext.IsActive = false;

                var updatedAccountVoewModel = _mapper.Map<AccountViewModel>(accountFromContext);

                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.UpdateAccountAsync(updatedAccountVoewModel).GetAwaiter().GetResult();

                updatedAccount = context.Accounts.LastOrDefault();
            }

            // Assert
            Assert.True(result);
            Assert.NotEqual(baseAccount.Login, updatedAccount.Login);
            Assert.NotEqual(baseAccount.Email, updatedAccount.Email);
            Assert.NotEqual(baseAccount.Password, updatedAccount.Password);
            Assert.NotEqual(baseAccount.Role, updatedAccount.Role);
            Assert.NotEqual(baseAccount.IsActive, updatedAccount.IsActive);
        }

        /// <summary>
        /// Тест на обновление данных пользователя, если указанный Id не найден.
        /// </summary>
        [Fact]
        public void UpdateAccount_Return_False()
        {
            // Arrange
            var options = GetContextOptions();
            var account = new AccountViewModel
            {
                Id = Guid.NewGuid(),
                Login = "Login",
                Email = "Email@Email.Email",
                Password = "P@ssword",
                Role = 1,
                IsActive = true
            };

            var result = false;

            // Act
            using (var context = new IdentityContext(options))
            {
                IAccountService accountService = new AccountService(_options, context, _mapper);
                result = accountService.UpdateAccountAsync(account).GetAwaiter().GetResult();
            }

            // Assert
            Assert.False(result);
        }
    }
}