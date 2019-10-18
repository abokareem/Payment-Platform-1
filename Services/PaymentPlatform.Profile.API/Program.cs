using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using PaymentPlatform.Framework.Services.SerilogLogger.Implementations;
using PaymentPlatform.Framework.Services.SerilogLogger.Interfaces;
using Serilog;
using System;

namespace PaymentPlatform.Profile.API
{
    public class Program
    {
        private static readonly string url = "http://*:49060";

        public static void Main(string[] args)
        {
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
