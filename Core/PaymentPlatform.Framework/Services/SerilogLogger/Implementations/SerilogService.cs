using PaymentPlatform.Framework.Services.SerilogLogger.Interfaces;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PaymentPlatform.Framework.Services.SerilogLogger.Implementations
{
    public class SerilogService : ISerilogService
    {
        public Logger SerilogConfiguration()
        {
            // TODO: Вынести (забирать) из конфигураций. Создать таблицу для Serilog.
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true";
            var tableName = "Serilog";

            var serilogConfig = new LoggerConfiguration()
                            .MinimumLevel.Information()
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Razor", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                            .MinimumLevel.Override("System", LogEventLevel.Warning)
                            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                            .WriteTo.MSSqlServer(connectionString, tableName, schemaName: "log")
                            .CreateLogger();

            return serilogConfig;
        }
    }
}
