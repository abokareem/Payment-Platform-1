using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaymentPlatform.Identity.API.Helpers;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Interfaces;
using PaymentPlatform.Identity.API.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        /// <inheritdoc/>
        public async Task<bool> UpdateAccountAsync(AccountViewModel accountViewModel)
        {
            var account = await _identityContext.Accounts.FirstOrDefaultAsync(p => p.Id == accountViewModel.Id);

            if (account == null)
            {
                return false;
            }

            account.Login = accountViewModel.Login;
            account.Role = accountViewModel.Role.Value;
            account.IsActive = accountViewModel.IsActive.Value;
            account.Email = accountViewModel.Email;
            account.Password = accountViewModel.Password;

            _identityContext.Update(account);
            await _identityContext.SaveChangesAsync();

            return true;
        }

        /// <inheritdoc/>
        public async Task<AccountViewModel> GetAccountByEmailAsync(string email)
        {
            var account = await _identityContext.Accounts.FirstOrDefaultAsync(p => p.Email == email);
            var accountViewModel = _mapper.Map<AccountViewModel>(account);

            return accountViewModel;
        }

        /// <inheritdoc/>
        public async Task<AccountViewModel> GetAccountByIdAsync(Guid accoundId)
        {
            var account = await _identityContext.Accounts.FirstOrDefaultAsync(p => p.Id == accoundId);
            var accountViewModel = _mapper.Map<AccountViewModel>(account);

            return accountViewModel;
        }

        /// <inheritdoc/>
        public async Task<List<AccountViewModel>> GetAllAccountsAsync(int? take = null, int? skip = null)
        {
            var queriableListOfAccounts = _identityContext.Accounts.Select(x => x);

            if (take != null && take > 0 && skip != null && skip > 0)
            {
                queriableListOfAccounts = queriableListOfAccounts.Skip((int)skip).Take((int)take);
            }

            var listOfAccounts = await queriableListOfAccounts.ToListAsync();

            var listOfAccountsViewModels = new List<AccountViewModel>();

            foreach (var account in listOfAccounts)
            {
                var accountViewModel = _mapper.Map<AccountViewModel>(account);
                listOfAccountsViewModels.Add(accountViewModel);
            }

            return listOfAccountsViewModels;
        }
    }
}
