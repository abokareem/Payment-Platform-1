using PaymentPlatform.Profile.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Services.Interfaces
{
    interface IProfileService
    {
        /// <summary>
		/// Добавить новый профиль.
		/// </summary>
		/// <param name="profileViewModel">Модель профиля.</param>
		/// <param name="userViewModel">Пользователь.</param>
		/// <returns>Id профиля.</returns>
		Task<string> AddNewProfileAsync(ProfileViewModel profileViewModel, UserViewModel userViewModel);

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
        Task<List<ProfileViewModel>> GetAllProfilesAsyc(int? take = null, int? skip = null);

        /// <summary>
        /// Обновить свойства профиля.
        /// </summary>
        /// <param name="profileViewModel">Профиль.</param>
        /// <returns>Результат операции.</returns>
        Task<bool> UpdateProfileAsync(ProfileViewModel profileViewModel);
    }
}
