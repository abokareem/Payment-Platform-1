using Serilog.Core;

namespace PaymentPlatform.Framework.Services.SerilogLogger.Interfaces
{
    /// <summary>
    /// Интерфейс Serilog.
    /// </summary>
    public interface ISerilogService
    {
        /// <summary>
        /// Настройка конфигураций для Serilog.
        /// </summary>
        /// <returns>Конфигурации.</returns>
        Logger SerilogConfiguration();
    }
}