using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Identity.API.Helpers
{
    public static class AppExtensions
    {
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
