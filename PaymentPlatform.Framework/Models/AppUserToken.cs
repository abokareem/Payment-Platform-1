﻿namespace PaymentPlatform.Framework.Models
{
    /// <summary>
    /// Пользовательский токен.
    /// </summary>
    public class AppUserToken
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Роль в приложении
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Токен для аутентификации.
        /// </summary>
        public string Token { get; set; }
    }
}
