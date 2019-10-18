using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace PaymentPlatform.Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:81")
                .UseStartup<Startup>();
    }
}
