namespace PaymentPlatform.Identity.API.Helpers
{
    /// <summary>
    /// Методы расширения в проекте Identity.
    /// </summary>
    public static class AppExtensions
    {
        /// <summary>
        /// Конвертация роли.
        /// </summary>
        /// <param name="role">роль в цифровом формате.</param>
        /// <returns>Роль в текством формате.</returns>
        public static string ConvertRole(this int role)
        {
            var result = string.Empty;

            switch (role)
            {
                case (int)AppRoles.User: { result = AppConstants.ROLE_USER; } break;
                case (int)AppRoles.Admin: { result = AppConstants.ROLE_ADMIN; } break;

                default: { result = AppConstants.ROLE_NONE; } break;
            }

            return result;
        }
    }
}
