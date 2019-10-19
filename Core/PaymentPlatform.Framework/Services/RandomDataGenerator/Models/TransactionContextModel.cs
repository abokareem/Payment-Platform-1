using PaymentPlatform.Framework.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPlatform.Framework.Services.RandomDataGenerator.Models
{
    /// <summary>
    /// Модель транзакции для Context.
    /// </summary>
    [Table("Transaction")]
    public class TransactionContextModel : TransactionModel
    {
        /// <summary>
        /// Время совершения операции.
        /// </summary>
        public new DateTime TransactionTime { get; set; } = DateTime.Now;

        // Навигационные свойства.
        public ProductContextModel Product { get; set; }
        public ProfileContextModel Profile { get; set; }
        public BalanceReservedContextModel BalanceReserved { get; set; }
        public ProductReservedContextModel ProductReserved { get; set; }
    }
}
