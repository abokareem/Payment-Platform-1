namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Transaction Logger.
    /// </summary>
    public static class TransactionLoggerConstants
    {
        /// <summary>
        /// N кол-во транзакций.
        /// </summary>
        public static readonly string GET_TRANSACTIONS = "transactions received.";

        /// <summary>
        /// Транзакция найден.
        /// </summary>
        public static readonly string GET_TRANSACTION_FOUND = "transaction found.";

        /// <summary>
        /// Транзакция не найден.
        /// </summary>
        public static readonly string GET_TRANSACTION_NOT_FOUND = "transaction not found.";

        /// <summary>
        /// Транзакция обновлена.
        /// </summary>
        public static readonly string UPDATE_TRANSACTION_OK = "transaction successfully updated.";

        /// <summary>
        /// Транзакция не обновлена.
        /// </summary>
        public static readonly string UPDATE_TRANSACTION_CONFLICT = "transaction not updated.";

        /// <summary>
        /// Транзакция добавлена.
        /// </summary>
        public static readonly string ADD_TRANSACTION_OK = "transaction successfully added.";

        /// <summary>
        /// Транзакция не добавлена.
        /// </summary>
        public static readonly string ADD_TRANSACTION_CONFLICT = "transaction not added.";

        /// <summary>
        /// Транзакция отменена.
        /// </summary>
        public static readonly string REVERT_TRANSACTION_OK = "transaction successfully reverted.";

        /// <summary>
        /// Транзакция не отменена.
        /// </summary>
        public static readonly string REVERT_TRANSACTION_CONFLICT = "transaction not reverted.";
    }
}