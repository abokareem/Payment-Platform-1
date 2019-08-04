using PaymentPlatform.Identity.API.Models;
using System.Threading.Tasks;

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
        /// <param name="email">электронная почта.</param>
        /// <param name="password">пароль.</param>
        /// <returns>Результат аутентификации.</returns>
        Task<UserToken> AuthenticateAsync(string email, string password);

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="account">данные.</param>
        /// <returns>Результат регистрации.</returns>
        Task<(bool result, string message)> RegistrationAsync(Account account);
    }
}
