using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Profile.API.Models;
using PaymentPlatform.Profile.API.Services.Implementations;
using PaymentPlatform.Profile.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		/// Конструктор
		/// </summary>
		/// <param name="fixture">приспособление для внедрения DI и InMemoryDatabase.</param>
		public ProfileServiceTests (ServiceFixture fixture)
		{
			_serviceProvider = fixture.ServiceProvider;
			_mapper = _serviceProvider.GetRequiredService<IMapper> ( );

			_rabbitMQService = new Mock<IRabbitMQService> ( );
			_rabbitMQService.Setup ( rmq => rmq.SetListener ( It.IsAny<string> ( ), It.IsAny<Action<string>> ( ) ) ).Returns ( (true, string.Empty) );
		}

		[Fact]
		public void AddNewProfile_Return_Id()
		{
			//Arrange
			var options = GetContextOptions ( );
			var newProfile = new ProfileViewModel ( )
			{
				Balance = 1,
				BankBook = Guid.NewGuid ( ).ToString ( ),
				FirstName = Guid.NewGuid ( ).ToString ( ),
				LastName = Guid.NewGuid ( ).ToString ( ),
				SecondName = Guid.NewGuid ( ).ToString ( ),
				OrgName = Guid.NewGuid ( ).ToString ( ),
				IsSeller = false,
				OrgNumber = Guid.NewGuid ( ).ToString ( )
			};
			//Act
			var guid = string.Empty;
			var result = string.Empty;
			using ( var context = new ProfileContext ( options ) )
			{
				IProfileService profileService = new ProfileService ( context, _mapper, _rabbitMQService.Object );

				result = profileService.AddNewProfileAsync ( newProfile ).GetAwaiter ( ).GetResult ( ).result;

				guid = context.Profiles.FirstOrDefault().Id.ToString();
			}

			//Assert
			Assert.Equal ( guid, result );
		}


		/// <summary>
		/// Формирование настроек для ProductContext в InMemoryDatabase.
		/// </summary>
		/// <returns>Настройки ProductContext.</returns>
		private DbContextOptions<ProfileContext> GetContextOptions ( )
		{
			var options = new DbContextOptionsBuilder<ProfileContext> ( )
				.UseInMemoryDatabase ( databaseName: Guid.NewGuid ( ).ToString ( ) )
				.Options;

			return options;
		}
	}
}
