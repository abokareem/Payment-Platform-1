using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentPlatform.Identity.API.Helpers;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Interfaces;
using PaymentPlatform.Identity.API.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PaymentPlatform.Identity.API.Services.Implementations
{
	/// <summary>
	/// Сервис для учетной записи пользователя.
	/// </summary>
	public class AccountService : IAccountService
	{
		private readonly AppSettings _appSettings;
		private readonly IdentityContext _identityContext;
		private readonly IMapper _mapper;

		/// <summary>
		/// Конструктор с параметрами.
		/// </summary>
		/// <param name="appSettings">настройки проекта.</param>
		/// <param name="identityContext">контекст бд.</param>
		public AccountService(IOptions<AppSettings> appSettings, IdentityContext identityContext, IMapper mapper)
		{
			_appSettings = appSettings.Value;
			_identityContext = identityContext;
			_mapper = mapper;
		}

		/// <inheritdoc/>
		public async Task<(bool result, string message)> RegistrationAsync(AccountViewModel accountViewModel)
		{
			var user = await _identityContext.Accounts.FirstOrDefaultAsync(a => a.Email == accountViewModel.Email);

			if (user != null)
			{
				return (false, AppConstants.USER_EXIST);
			}

			var model = _mapper.Map<Account>(accountViewModel);

			await _identityContext.Accounts.AddAsync(model);
			await _identityContext.SaveChangesAsync();

			return (true, string.Empty);
		}

		/// <inheritdoc/>
		public async Task<(string access_token, string username, int role)?> AuthenticateAsync(LoginViewModel loginViewModel)
		{
			var account = await _identityContext.Accounts.SingleOrDefaultAsync(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password);

			if (account == null)
			{
				return null;
			}
			var now = DateTime.UtcNow;
			var identity = new
			{
				Claims = new Claim[]
				{
					new Claim("id", account.Id.ToString()),
					new Claim("role", account.Role.ConvertRole()),
				}
			};
			var authOptions = new AuthOptions();
			var jwt = new JwtSecurityToken(
					issuer: authOptions.ValidIssuer,
					audience: authOptions.ValidAudience,
					notBefore: now,
					claims: identity.Claims,
					expires: now.Add(TimeSpan.FromMinutes(authOptions.TokenLifetime)),
					signingCredentials: new SigningCredentials(authOptions.GetIssuerSigningKey(), SecurityAlgorithms.HmacSha256));
			var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

			var response = (encodedJwt, account.Login, account.Role);

			return response;
		}
	}
}
