using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Interfaces;
using System;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PaymentPlatform.Identity.API.Helpers;

namespace PaymentPlatform.Identity.API.Services.Implementations
{
    /// <summary>
    /// Сервис для учетной записи пользователя.
    /// </summary>
    public class AccountService : IAccountService
    {
        private readonly AppSettings _appSettings;
        private readonly IdentityContext _identityContext;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="appSettings">настройки проекта.</param>
        /// <param name="identityContext">контекст бд.</param>
        public AccountService(IOptions<AppSettings> appSettings, IdentityContext identityContext)
        {
            _appSettings = appSettings.Value;
            _identityContext = identityContext;
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="email">электронная почта.</param>
        /// <param name="password">пароль.</param>
        /// <returns>Результат аутентификации.</returns>
        public UserToken Authenticate(string email, string password)
        {
            var account = _identityContext.Accounts.SingleOrDefault(x => x.Email == email && x.Password == password);

            if (account is null)
            {
                return null;
            }     

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", account.Id.ToString()),
                    new Claim("role", account.Role.ConvertRole()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtSecurityToken = tokenHandler.WriteToken(token);

            return new UserToken() { JWT_Token = jwtSecurityToken };
        }
    }
}
