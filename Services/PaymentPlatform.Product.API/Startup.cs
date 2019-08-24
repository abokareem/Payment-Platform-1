﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentPlatform.Product.API.Helpers;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Implementations;
using PaymentPlatform.Product.API.Services.Interfaces;
using System.Text;

namespace PaymentPlatform.Product.API
{
	public class Startup
	{
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			string connectionString = Configuration.GetConnectionString("DefaultConnection");
			services.AddDbContext<ProductContext>(options => options.UseSqlServer(connectionString));

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //		.AddJwtBearer(options =>
            //		{
            //			//TODO: Вынести в конфиг
            //			options.RequireHttpsMetadata = false;
            //			options.TokenValidationParameters = new TokenValidationParameters
            //			{
            //				// укзывает, будет ли валидироваться издатель при валидации токена
            //				ValidateIssuer = true,
            //				// строка, представляющая издателя
            //				ValidIssuer = "http://localhost:49051",

            //				// будет ли валидироваться потребитель токена
            //				ValidateAudience = true,
            //				// установка потребителя токена
            //				ValidAudience = "PaymentPlatform",
            //				// будет ли валидироваться время существования
            //				ValidateLifetime = true,

            //				// установка ключа безопасности
            //				IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3ce1637ed40041cd94d4853d3e766c4d")),
            //				// валидация ключа безопасности
            //				ValidateIssuerSigningKey = true,
            //			};
            //		});

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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3ce1637ed40041cd94d4853d3e766c4d")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            var mappingConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MappingProfile());
			});

			IMapper mapper = mappingConfig.CreateMapper();
			services.AddSingleton(mapper);

			services.AddScoped<IProductService, ProductService>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

            app.UseAuthentication();
            app.UseMvc();
		}
	}
}
