using PaymentPlatform.Profile.API.Services.Interfaces;
using PaymentPlatform.Profile.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Services.Implementations
{
    /// <summary>
    /// Реализация сервиса Profile.
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <inheritdoc/>
        public Task<string> AddNewProfileAsync(ProfileViewModel profileViewModel, UserViewModel userViewModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<List<ProfileViewModel>> GetAllProfilesAsyc(int? take = null, int? skip = null)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<ProfileViewModel> GetProfileByIdAsync(Guid profileId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<bool> UpdateProfileAsync(ProfileViewModel profileViewModel)
        {
            throw new NotImplementedException();
        }
    }
}
