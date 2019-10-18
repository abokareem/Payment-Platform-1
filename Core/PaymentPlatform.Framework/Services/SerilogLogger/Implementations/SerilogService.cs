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
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=PaymentPlatformApplication;Trusted_Connection=True;MultipleActiveResultSets=true";
            var tableName = "Serilog";

            //var columnOption = new ColumnOptions();
            //columnOption.Store.Remove(StandardColumn.MessageTemplate);
            //columnOption.Store.Remove(StandardColumn.Properties);

            var serilogConfig =
                new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Razor", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Mvc.Razor.Internal", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.MSSqlServer(connectionString,
                                     tableName,
                                     //columnOptions: columnOption,
                                     //autoCreateSqlTable: true,
                                     schemaName: "log")
                .CreateLogger();

            return serilogConfig;
        }
    }
}
