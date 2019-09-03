using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;
using System;
using System.Collections.Generic;
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
        Task<AppUserToken> AuthenticateAsync(LoginViewModel loginViewModel);

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
        /// Получить учетную запись пользователя по его Guid.
        /// </summary>
        /// <param name="accoundId">Guid учетной записи.</param>
        /// <returns>ViewModel учетная запись пользователя.</returns>
        Task<AccountViewModel> GetAccountByIdAsync(Guid accoundId);

        /// <summary>
        /// Получить все учетные записи пользователей.
        /// </summary>
        /// <param name="take">параметр пагинации (кол-во взять).</param>
        /// <param name="skip">параметр пагинации (кол-во пропустить).</param>
        /// <returns>Список учетных записей пользователей.</returns>
        Task<List<AccountViewModel>> GetAllAccountsAsync(int? take = null, int? skip = null);

        /// <summary>
        /// Обновить данные пользователя.
        /// </summary>
        /// <param name="accountViewModel">данные учетной записи пользователя.</param>
        /// <returns>Результат операции.</returns>
        Task<bool> UpdateAccountAsync(AccountViewModel accountViewModel);
    }
}
