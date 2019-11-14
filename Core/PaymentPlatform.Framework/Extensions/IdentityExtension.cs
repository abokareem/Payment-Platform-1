using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Enums;

namespace PaymentPlatform.Framework.Extensions
{
    /// <summary>
    /// Метод расширения для конвертации роли в проекте Identity.
    /// </summary>
    public static class ConvertRoleExtension
    {
        /// <summary>
        /// Конвертация роли.
        /// </summary>
        /// <param name="role">роль в цифровом формате.</param>
        /// <returns>Роль в текством формате.</returns>
        public static string ConvertRole(this int role)
        {
            string result;

            switch (role)
            {
                case (int)IdentityRoles.User: { result = IdentityConstants.ROLE_USER; } break;
                case (int)IdentityRoles.Admin: { result = IdentityConstants.ROLE_ADMIN; } break;

                default: { result = IdentityConstants.ROLE_NONE; } break;
            }

            return result;
        }
    }
}
