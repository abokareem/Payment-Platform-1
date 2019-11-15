using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PaymentPlatform.Framework.Services.SerilogLogger.Implementations;
using PaymentPlatform.Framework.Services.SerilogLogger.Interfaces;
using Serilog;
using System;

namespace PaymentPlatform.Identity.API
{
    public class Program
    {
        private static readonly string url = "http://*:85";

        public static void Main(string[] args)
        {
            // Реализация: http://ajitgoel.net/configuring-serilog-sql-server-sink-logging-for-asp-net-core-application/
            //var path = Directory.GetCurrentDirectory();
            //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

            //var configuration = new ConfigurationBuilder()
            //.SetBasePath(path)
            //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //.Build();

            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            //    .Enrich.FromLogContext()
            //    .ReadFrom.Configuration(configuration)
            //    .CreateLogger();

            ISerilogService serilogConfiguration = new SerilogService();
            Log.Logger = serilogConfiguration.SerilogConfiguration();

            try
            {
                Log.Information($"Server on {url} loaded successfully.");
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.Information($"Server on {url} stopped successfully.");
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(url)
                .UseSerilog()
                .UseStartup<Startup>();
    }
}