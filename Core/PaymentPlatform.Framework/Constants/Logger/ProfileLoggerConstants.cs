namespace PaymentPlatform.Framework.Constants.Logger
{
    /// <summary>
    /// Константы для Identity Logger.
    /// </summary>
    public static class ProfileLoggerConstants
    {
        /// <summary>
        /// N кол-во профилей.
        /// </summary>
        public static readonly string GET_PROFILES = "profiles received.";

        /// <summary>
        /// Профиль найден.
        /// </summary>
        public static readonly string GET_PROFILE_FOUND = "profile found.";

        /// <summary>
        /// Профиль не найден.
        /// </summary>
        public static readonly string GET_PROFILE_NOT_FOUND = "profile not found.";

        /// <summary>
        /// Профиль добавлен.
        /// </summary>
        public static readonly string ADD_PROFILE_OK = "profile successfully added.";

        /// <summary>
        /// Профиль не добавлен.
        /// </summary>
        public static readonly string ADD_PROFILE_CONFLICT = "profile not added.";

        /// <summary>
        /// Профиль обновлен.
        /// </summary>
        public static readonly string UPDATE_PROFILE_OK = "profile successfully updated.";

        /// <summary>
        /// Профиль не обновлен.
        /// </summary>
        public static readonly string UPDATE_PROFILE_CONFLICT = "profile not updated.";
    }
}