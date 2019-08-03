using PaymentPlatform.Identity.API.Models;

namespace PaymentPlatform.Identity.API.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для DI.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="username">псевдоним.</param>
        /// <param name="password">пароль.</param>
        /// <returns></returns>
        UserToken Authenticate(string email, string password);
    }
}
