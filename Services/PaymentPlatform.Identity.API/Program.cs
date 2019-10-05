using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;

namespace PaymentPlatform.Identity.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true";
            var tableName = "Serilog";

            var columnOption = new ColumnOptions();
            columnOption.Store.Remove(StandardColumn.MessageTemplate);

            Log.Logger = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Razor", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .WriteTo.MSSqlServer(connectionString, tableName, columnOptions: columnOption)
                            .CreateLogger();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:49060")
                .UseSerilog()
                .UseStartup<Startup>();
    }
}
