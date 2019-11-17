using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PaymentPlatform.Framework.Extensions
{
    /// <summary>
    /// Метод расширения для добавления сервиса авторизации на основе JWT токена.
    /// </summary>
    public static class JwtServiceConfigurationExtension
    {
        /// <summary>
        /// Добавление сервиса авторизации на основе JWT токена.
        /// </summary>
        /// <param name="services">DI контейнер.</param>
        /// <param name="secret">секретный ключ.</param>
        public static void AddJwtService(this IServiceCollection services, string secret)
        {
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}