namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Identity Logger.
    /// </summary>
    public static class IdentityLoggerConstants
    {
        /// <summary>
        /// N кол-во аккаунтов.
        /// </summary>
        public static readonly string GET_ACCOUNTS = "accounts received.";

        /// <summary>
        /// Аккаунт не найден.
        /// </summary>
        public static readonly string GET_ACCOUNT_NOT_FOUND = "account not found.";

        /// <summary>
        /// Аккаунт найден.
        /// </summary>
        public static readonly string GET_ACCOUNT_FOUND = "account found.";

        /// <summary>
        /// Эл. адрес найден.
        /// </summary>
        public static readonly string GET_EMAIL_FOUND = "email found.";

        /// <summary>
        /// Эл. адрес не найден.
        /// </summary>
        public static readonly string GET_EMAIL_NOT_FOUND = "email not found.";

        /// <summary>
        /// Эл. адрес существует.
        /// </summary>
        public static readonly string EMAIL_EXIST = "already exists.";

        /// <summary>
        /// Эл. адрес не существует.
        /// </summary>
        public static readonly string EMAIL_NOT_EXIST = "not exists.";

        /// <summary>
        /// Эл. адрес успешно зарегистрирован.
        /// </summary>
        public static readonly string EMAIL_REGISTRATION_OK = "successfully registered.";

        /// <summary>
        /// Аккаунт обновлен.
        /// </summary>
        public static readonly string EMAIL_UPDATE_OK = "successfully updated.";

        /// <summary>
        /// Аккаунт не обновлен.
        /// </summary>
        public static readonly string UPDATE_EMAIL_CONFLICT = "not updated.";
    }
}