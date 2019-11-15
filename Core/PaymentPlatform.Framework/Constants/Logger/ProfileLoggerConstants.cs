namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Identity Logger.
    /// </summary>
    public class ProfileLoggerConstants
    {
        /// <summary>
        /// N кол-во профилей.
        /// </summary>
        public const string GET_PROFILES = "profiles received.";

        /// <summary>
        /// Профиль найден.
        /// </summary>
        public const string GET_PROFILE_FOUND = "profile found.";

        /// <summary>
        /// Профиль не найден.
        /// </summary>
        public const string GET_PROFILE_NOT_FOUND = "profile not found.";

        /// <summary>
        /// Профиль добавлен.
        /// </summary>
        public const string ADD_PROFILE_OK = "profile successfully added.";

        /// <summary>
        /// Профиль не добавлен.
        /// </summary>
        public const string ADD_PROFILE_CONFLICT = "profile not added.";

        /// <summary>
        /// Профиль обновлен.
        /// </summary>
        public const string UPDATE_PROFILE_OK = "profile successfully updated.";

        /// <summary>
        /// Профиль не обновлен.
        /// </summary>
        public const string UPDATE_PROFILE_CONFLICT = "profile not updated.";
    }
}