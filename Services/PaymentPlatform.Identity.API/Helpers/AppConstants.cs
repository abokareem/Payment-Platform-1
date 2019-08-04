namespace PaymentPlatform.Identity.API.Helpers
{
    /// <summary>
    /// Константы в проекте Identity.
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Роль отсутствует.
        /// </summary>
        public static readonly string ROLE_NONE = "None";

        /// <summary>
        /// Пользователь.
        /// </summary>
        public static readonly string ROLE_USER = "User";

        /// <summary>
        /// Администратор.
        /// </summary>
        public static readonly string ROLE_ADMIN = "Admin";

        /// <summary>
        /// Пользователь существует.
        /// </summary>
        public static readonly string USER_EXIST = "Пользователь c таким электронным адресом уже существует.";

        /// <summary>
        /// Пользователь существует.
        /// </summary>
        public static readonly string USER_DATA_INCORRECT = "Логин или пароль неверны.";
    }
}
