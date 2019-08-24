using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PaymentPlatform.Gateway.API.Helpers
{
    public class AuthOptions
    {
        public string ValidIssuer { get; } = "http://localhost:49051";
        public string ValidAudience { get; } = "PaymentPlatform";
        public string Key { get; } = "3ce1637ed40041cd94d4853d3e766c4d";
        public int TokenLifetime { get; } = 60;
        public SymmetricSecurityKey GetIssuerSigningKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
