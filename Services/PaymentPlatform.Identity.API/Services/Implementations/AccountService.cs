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
        //TODO: Удалить неиспользуемые свойства.
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
			//_appSettings = appSettings.Value;
			_identityContext = identityContext;
			_mapper = mapper;
            _appSettings = appSettings.Value;
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

			return (true, AppConstants.USER_REGISTRATION_SUCCESS);
		}

		/// <inheritdoc/>
		public async Task<UserToken> AuthenticateAsync(LoginViewModel loginViewModel)
		{
			var account = await _identityContext.Accounts.SingleOrDefaultAsync(x => x.Email == loginViewModel.Email && x.Password == loginViewModel.Password);

			if (account == null)
			{
				return null;
			}

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, account.Id.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, account.Role.ConvertRole())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtSecurityToken = tokenHandler.WriteToken(token);

            var userToken = new UserToken()
            {
                UserName = account.Login,
                Role = account.Role.ConvertRole(),
                Token = jwtSecurityToken
            };

            return userToken;
        }
    }
}
