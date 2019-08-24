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
        Task<UserToken> AuthenticateAsync(LoginViewModel loginViewModel);

        /// <summary>
        /// Регистрация пользователя.
        /// </summary>
        /// <param name="accountViewModel">данные учетной записи пользователя.</param>
        /// <returns>Результат регистрации.</returns>
        Task<(bool result, string message)> RegistrationAsync(AccountViewModel accountViewModel);

        /// <summary>
        /// Получить данные пользователя по Email.
        /// </summary>
        /// <param name="email">электронный адрес.</param>
        /// <returns>ViewModel данных пользователя.</returns>
        Task<AccountViewModel> GetAccountByEmailAsync(string email);

        /// <summary>
        /// Обновить данные пользователя.
        /// </summary>
        /// <param name="accountViewModel">данные учетной записи пользователя.</param>
        /// <returns>Результат операции.</returns>
        Task<bool> UpdateAccountAsync(AccountViewModel accountViewModel);
    }
}
