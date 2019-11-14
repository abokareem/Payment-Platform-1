using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentPlatform.Framework.Extensions;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Mapping;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Implementations;
using PaymentPlatform.Identity.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

namespace PaymentPlatform.Identity.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var appSettingSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingSection);

            var appSettings = appSettingSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var isProduction = appSettings.IsProduction;

            services.AddJwtService(appSettings.Secret);

            string connectionString = Configuration.GetConnectionString(isProduction.ToDbConnectionString());
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AccountProfile());
            });

            var mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IAccountService, AccountService>();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "PaymentPlatform Identity API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentPlatform Identity API version 1"));

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
