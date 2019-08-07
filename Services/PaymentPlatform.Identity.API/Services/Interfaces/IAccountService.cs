using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.ViewModels;
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
        /// <param name="loginViewModel">данные пользователя.</param>
        /// <returns>Результат аутентификации.</returns>
        Task<(string access_token, string username, int role)?> AuthenticateAsync(LoginViewModel loginViewModel);

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="accountViewModel">данные учетной записи пользователя.</param>
        /// <returns>Результат регистрации.</returns>
        Task<(bool result, string message)> RegistrationAsync(AccountViewModel accountViewModel);
    }
}
