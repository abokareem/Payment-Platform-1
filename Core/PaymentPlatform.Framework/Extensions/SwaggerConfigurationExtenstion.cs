using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;

namespace PaymentPlatform.Framework.Extensions
{
    /// <summary>
    /// Метод расширения для добавления сервиса SwaggerGen.
    /// </summary>
    public static class SwaggerServiceConfigurationExtenstion
    {
        /// <summary>
        /// Добавление сервиса SwaggerGen.
        /// </summary>
        /// <param name="services">DI контейнер.</param>
        /// <param name="title">название для SwaggerDoc.</param>
        public static void AddSwaggerService(this IServiceCollection services, string title)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = title, Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0] }
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
            });
        }
    }
}
