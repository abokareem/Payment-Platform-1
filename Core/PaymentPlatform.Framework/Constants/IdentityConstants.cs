﻿namespace PaymentPlatform.Framework.Constants
{
    /// <summary>
    /// Константы для Identity.
    /// </summary>
    public static class IdentityConstants
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
        public static readonly string USER_REGISTRATION_SUCCESS = "Пользователь успешно зарегистрирован.";

        /// <summary>
        /// Пользователь существует.
        /// </summary>
        public static readonly string USER_DATA_INCORRECT = "Логин или пароль неверны.";
    }
}