using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PaymentPlatform.Gateway.API
{
    public class Program
    {
        private static readonly string url = "http://*:81";

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls(url)
                .UseStartup<Startup>();
    }
}