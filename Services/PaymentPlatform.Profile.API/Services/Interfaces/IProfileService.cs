using PaymentPlatform.Profile.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Services.Interfaces
{
    /// <summary>
    /// Интерфейс для сервиса Profile.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
		/// Добавить новый профиль.
		/// </summary>
		/// <param name="profileViewModel">профиля.</param>
		/// <returns>Id профиля.</returns>
		Task<(string result, bool success)> AddNewProfileAsync(ProfileViewModel profileViewModel);

        /// <summary>
        /// Получить профиль по его Id.
        /// </summary>
        /// <param name="profileId">Id профиля.</param>
        /// <returns>ViewModel профиля.</returns>
        Task<ProfileViewModel> GetProfileByIdAsync(Guid profileId);

        /// <summary>
        /// Получить все профили.
        /// </summary>
        /// <param name="take">параметр пагинации (кол-во взять).</param>
        /// <param name="skip">параметр пагинации (кол-во пропустить).</param>
        /// <returns>Список профилей.</returns>
        Task<List<ProfileViewModel>> GetAllProfilesAsync(int? take = null, int? skip = null);

        /// <summary>
        /// Обновить профиль.
        /// </summary>
        /// <param name="profileViewModel">профиль.</param>
        /// <returns>Результат операции.</returns>
        Task<bool> UpdateProfileAsync(ProfileViewModel profileViewModel);
    }
}
