using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using PaymentPlatform.Gateway.API.Helpers;
using PaymentPlatform.Identity.API.Helpers;
using System.Text;

namespace PaymentPlatform.Gateway.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("configuration.json")
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);

            var appSettings = appSettingSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            var authOtions = new AuthOptions();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                //TODO: Вынести в конфиг
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authOtions.ValidIssuer,
                    ValidateAudience = true,
                    ValidAudience = authOtions.ValidAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = authOtions.GetIssuerSigningKey(),
                    ValidateIssuerSigningKey = true,
                };
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseOcelot().Wait();
            app.UseMvc();
        }
    }
}
