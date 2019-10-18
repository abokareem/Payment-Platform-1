namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Identity Logger.
    /// </summary>
    public class IdentityLoggerConstants
    {
        /// <summary>
        /// N кол-во аккаунтов.
        /// </summary>
        public const string GET_ACCOUNTS = "accounts received.";

        /// <summary>
        /// Аккаунт не найден.
        /// </summary>
        public const string GET_ACCOUNT_NOT_FOUND = "account not found.";

        /// <summary>
        /// Аккаунт найден.
        /// </summary>
        public const string GET_ACCOUNT_FOUND = "account found.";

        /// <summary>
        /// Эл. адрес найден.
        /// </summary>
        public const string GET_EMAIL_FOUND = "email found.";

        /// <summary>
        /// Эл. адрес не найден.
        /// </summary>
        public const string GET_EMAIL_NOT_FOUND = "email not found.";

        /// <summary>
        /// Эл. адрес существует.
        /// </summary>
        public const string EMAIL_EXIST = "already exists.";

        /// <summary>
        /// Эл. адрес не существует.
        /// </summary>
        public const string EMAIL_NOT_EXIST = "not exists.";

        /// <summary>
        /// Эл. адрес успешно зарегистрирован.
        /// </summary>
        public const string EMAIL_REGISTRATION_OK = "successfully registered.";

        /// <summary>
        /// Аккаунт обновлен.
        /// </summary>
        public const string EMAIL_UPDATE_OK = "successfully updated.";

        /// <summary>
        /// Аккаунт не обновлен.
        /// </summary>
        public const string UPDATE_EMAIL_CONFLICT = "not updated.";
    }
}
