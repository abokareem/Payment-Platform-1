using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentPlatform.Framework.Extensions;
using PaymentPlatform.Framework.Helpers;
using PaymentPlatform.Framework.Mapping;
using PaymentPlatform.Framework.Services.RabbitMQ.Implementations;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Implementations;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using Swashbuckle.AspNetCore.Swagger;
using System.Text;

namespace PaymentPlatform.Transaction.API
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
            services.AddDbContext<TransactionContext>(options => options.UseSqlServer(connectionString));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new TransactionProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddSingleton<IRabbitMQService, RabbitMQService>();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "PaymentPlatform Transaction API", Version = "v1" }));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PaymentPlatform Transaction API version 1"));

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
