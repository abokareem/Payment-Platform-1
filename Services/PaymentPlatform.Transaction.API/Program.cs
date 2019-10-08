using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PaymentPlatform.Framework.Services.SerilogLogger.Implementations;
using PaymentPlatform.Framework.Services.SerilogLogger.Interfaces;
using Serilog;

namespace PaymentPlatform.Transaction.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
			ISerilogService serilogConfiguration = new SerilogService();
			Log.Logger = serilogConfiguration.SerilogConfiguration();

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
                .UseUrls("http://*:49090")
                .UseStartup<Startup>();
    }
}
