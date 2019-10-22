using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Models
{
    /// <summary>
    /// Модель зарезервированного баланса  для Context.
    /// </summary>
    public class BalanceReservedContextModel : BalanceReservedModel
    {
        // Навигационные свойства
        public ProfileContextModel Profile { get; set; }
        public TransactionContextModel Transaction { get; set; }
    }
}
