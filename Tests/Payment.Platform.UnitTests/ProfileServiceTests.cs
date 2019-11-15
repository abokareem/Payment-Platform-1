using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Profile.API.Models;
using PaymentPlatform.Profile.API.Services.Implementations;
using PaymentPlatform.Profile.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Payment.Platform.UnitTests
{
    /// <summary>
    /// Класс тестов для класса ProfileService
    /// </summary>
    public class ProfileServiceTests : IClassFixture<ServiceFixture>
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly Mock<IRabbitMQService> _rabbitMQService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
        public ProfileServiceTests(ServiceFixture fixture)
        {
            _serviceProvider = fixture.ServiceProvider;
            _mapper = _serviceProvider.GetRequiredService<IMapper>();

            _rabbitMQService = new Mock<IRabbitMQService>();
            _rabbitMQService.Setup(rmq => rmq.SetListener(It.IsAny<string>(), It.IsAny<Action<string>>())).Returns((true, string.Empty));
        }

        /// <summary>
        /// Формирование настроек для ProductContext в InMemoryDatabase.
        /// </summary>
        /// <returns>Настройки ProductContext.</returns>
        private DbContextOptions<ProfileContext> GetContextOptions()
        {
            var options = new DbContextOptionsBuilder<ProfileContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return options;
        }

        /// <summary>
        /// Тест на добавление нового профиля.
        /// </summary>
        [Fact]
        public void AddNewProfile_Return_IdAndTrue()
        {
            //Arrange
            var options = GetContextOptions();
            var newProfile = new ProfileViewModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = Guid.NewGuid().ToString(),
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            var guid = string.Empty;
            var result = string.Empty;
            var success = false;

            //Act
            using (var context = new ProfileContext(options))
            {
                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                (result, success) = profileService.AddNewProfileAsync(newProfile).GetAwaiter().GetResult();

                guid = context.Profiles.FirstOrDefault().Id.ToString();
            }

            //Assert
            Assert.True(success);
            Assert.Equal(guid, result);
        }

        /// <summary>
        /// Тест на добавление нового профиля.
        /// </summary>
        [Fact]
        public void AddNewProfile_Return_IdAndFalse()
        {
            //Arrange
            var options = GetContextOptions();

            var profileModel = new ProfileModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = "Passport",
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            var profileViewModel = new ProfileViewModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = "Passport",
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            var guid = string.Empty;
            var result = string.Empty;
            var success = false;

            //Act
            using (var context = new ProfileContext(options))
            {
                context.Profiles.Add(profileModel);
                context.SaveChanges();

                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                (result, success) = profileService.AddNewProfileAsync(profileViewModel).GetAwaiter().GetResult();
            }

            //Assert
            Assert.False(success);
            Assert.Equal(GlobalConstants.PROFILE_SERVICE_FAIL, result);
        }

        /// <summary>
        /// Тест на получение профиля по его идентификатору.
        /// </summary>
        [Fact]
        public void GetProfileById_Return_Profile()
        {
            //Arrange
            var options = GetContextOptions();
            var expectedProfile = new ProfileModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = Guid.NewGuid().ToString(),
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            ProfileViewModel actualProfile;

            //Act
            using (var context = new ProfileContext(options))
            {
                context.Profiles.Add(expectedProfile);
                context.SaveChanges();

                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                actualProfile = profileService.GetProfileByIdAsync(expectedProfile.Id).GetAwaiter().GetResult();
            }

            //Assert
            Assert.Equal(expectedProfile.Id, actualProfile.Id);
            Assert.Equal(expectedProfile.IsSeller, actualProfile.IsSeller);
            Assert.Equal(expectedProfile.LastName, actualProfile.LastName);
            Assert.Equal(expectedProfile.FirstName, actualProfile.FirstName);
            Assert.Equal(expectedProfile.SecondName, actualProfile.SecondName);
            Assert.Equal(expectedProfile.OrgName, actualProfile.OrgName);
            Assert.Equal(expectedProfile.OrgNumber, actualProfile.OrgNumber);
            Assert.Equal(expectedProfile.Balance, actualProfile.Balance);
            Assert.Equal(expectedProfile.BankBook, actualProfile.BankBook);
        }

        /// <summary>
        /// Тест на получение профиля по несуществующему идентификатору.
        /// </summary>
        [Fact]
        public void GetProfileById_Return_Null()
        {
            //Arrange
            var options = GetContextOptions();
            var guid = Guid.NewGuid();

            //Act
            ProfileViewModel actualProfile;

            using (var context = new ProfileContext(options))
            {
                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                actualProfile = profileService.GetProfileByIdAsync(guid).GetAwaiter().GetResult();
            }

            //Assert
            Assert.Null(actualProfile);
        }

        /// <summary>
        /// Тест на получение всех профилей.
        /// </summary>
        [Fact]
        public void GetAllProfiles_Return_Profiles()
        {
            //Arrange
            var options = GetContextOptions();

            #region New profiles

            var profileOne = new ProfileModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = Guid.NewGuid().ToString(),
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            var profileTwo = new ProfileModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = Guid.NewGuid().ToString(),
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            var profileThree = new ProfileModel()
            {
                Balance = 1,
                BankBook = Guid.NewGuid().ToString(),
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                SecondName = Guid.NewGuid().ToString(),
                Passport = Guid.NewGuid().ToString(),
                OrgName = Guid.NewGuid().ToString(),
                IsSeller = false,
                OrgNumber = Guid.NewGuid().ToString()
            };

            #endregion New profiles

            List<ProfileViewModel> actualProfiles;

            //Act
            using (var context = new ProfileContext(options))
            {
                context.Profiles.Add(profileOne);
                context.Profiles.Add(profileTwo);
                context.Profiles.Add(profileThree);
                context.SaveChanges();

                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);

                actualProfiles = profileService.GetAllProfilesAsync().GetAwaiter().GetResult().ToList();
            }

            //Assert
            Assert.Equal(3, actualProfiles.Count);
        }

        /// <summary>
        /// Тест на получение всех профилей, если профилей не существует.
        /// </summary>
        [Fact]
        public void GetAllProfiles_Return_Null()
        {
            //Arrange
            var options = GetContextOptions();
            //Act
            var actualProfiles = new List<ProfileViewModel>();

            using (var context = new ProfileContext(options))
            {
                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);

                actualProfiles = profileService.GetAllProfilesAsync().GetAwaiter().GetResult().ToList();
            }

            //Assert
            Assert.Empty(actualProfiles);
        }

        /// <summary>
        /// Тест на обновление профиля.
        /// </summary>
        [Fact]
        public void UpdateProfile_Return_True()
        {
            //Arrange
            var options = GetContextOptions();

            var newProfile = new ProfileModel()
            {
                Balance = 1,
                BankBook = "BankBook",
                FirstName = "FirstName",
                LastName = "LastName",
                SecondName = "SecondName",
                Passport = "Passport",
                OrgName = "OrgName",
                IsSeller = false,
                OrgNumber = "OrgNumber"
            };

            ProfileModel oldProfile;
            ProfileModel updatedProfile;

            var result = false;

            //Act
            using (var context = new ProfileContext(options))
            {
                context.Profiles.Add(newProfile);
                context.SaveChanges();

                oldProfile = context.Profiles.AsNoTracking().LastOrDefault();
                updatedProfile = context.Profiles.LastOrDefault();
                updatedProfile.Balance = 2;
                updatedProfile.BankBook = Guid.NewGuid().ToString();
                updatedProfile.FirstName = Guid.NewGuid().ToString();
                updatedProfile.IsSeller = true;
                updatedProfile.LastName = Guid.NewGuid().ToString();
                updatedProfile.SecondName = Guid.NewGuid().ToString();
                updatedProfile.Passport = Guid.NewGuid().ToString();
                updatedProfile.OrgName = Guid.NewGuid().ToString();
                updatedProfile.OrgNumber = Guid.NewGuid().ToString();

                var updatedProfileViewModel = _mapper.Map<ProfileViewModel>(updatedProfile);

                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                result = profileService.UpdateProfileAsync(updatedProfileViewModel).GetAwaiter().GetResult();

                updatedProfile = context.Profiles.LastOrDefault();
            }

            //Assert
            Assert.True(result);
            Assert.NotEqual(oldProfile.Balance, updatedProfile.Balance);
            Assert.NotEqual(oldProfile.BankBook, updatedProfile.BankBook);
            Assert.NotEqual(oldProfile.FirstName, updatedProfile.FirstName);
            Assert.NotEqual(oldProfile.SecondName, updatedProfile.SecondName);
            Assert.NotEqual(oldProfile.LastName, updatedProfile.LastName);
            Assert.NotEqual(oldProfile.OrgName, updatedProfile.OrgName);
            Assert.NotEqual(oldProfile.OrgNumber, updatedProfile.OrgNumber);
            Assert.NotEqual(oldProfile.IsSeller, updatedProfile.IsSeller);
        }

        /// <summary>
        /// Тест на обновление несуществующего профиля.
        /// </summary>
        [Fact]
        public void UpdateProfile_Return_False()
        {
            //Arrange
            var options = GetContextOptions();
            var fakeProfile = new ProfileViewModel()
            {
                Id = Guid.NewGuid()
            };

            var result = false;

            //Act
            using (var context = new ProfileContext(options))
            {
                IProfileService profileService = new ProfileService(context, _mapper, _rabbitMQService.Object);
                result = profileService.UpdateProfileAsync(fakeProfile).GetAwaiter().GetResult();
            }

            //Assert
            Assert.False(result);
        }
    }
}