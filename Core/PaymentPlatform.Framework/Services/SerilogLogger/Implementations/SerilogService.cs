using Microsoft.Extensions.Configuration;
using PaymentPlatform.Framework.Services.SerilogLogger.Interfaces;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace PaymentPlatform.Framework.Services.SerilogLogger.Implementations
{
    /// <summary>
    /// Сервис для настройки Serilog.
    /// </summary>
    public class SerilogService : ISerilogService
    {
        /// <summary>
        /// Готовая конфигурация для Serilog.
        /// </summary>
        /// <returns>Конфигурация Seriog.</returns>
        public Logger SerilogConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //var settings = new SerilogConfig();
            //configuration.Bind(settings);

            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            var tableName = "Serilog";

            //var object1 = new SqlColumn { ColumnName = "OtherData", DataType = SqlDbType.NVarChar, DataLength = 64 };
            //var object2 = new SqlColumn { ColumnName = "AnotherData", DataType = SqlDbType.NVarChar, DataLength = 64 };

            //var columnOption = new ColumnOptions();
            //columnOption.Store.Remove(StandardColumn.MessageTemplate);
            //columnOption.Store.Remove(StandardColumn.Properties);

            //columnOption.AdditionalColumns = new Collection<SqlColumn>();
            //columnOption.AdditionalColumns.Add(object1);
            //columnOption.AdditionalColumns.Add(object2);

            var serilogConfig =
                new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .WriteTo.MSSqlServer(connectionString,
                                     tableName,
                                     //columnOptions: columnOption,
                                     //schemaName: "log",
                                     autoCreateSqlTable: true)
                .CreateLogger();

            return serilogConfig;
        }
    }
}