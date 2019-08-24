using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Profile.API.Models;
using PaymentPlatform.Profile.API.Services.Interfaces;
using PaymentPlatform.Profile.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Profile.API.Services.Implementations
{
    /// <summary>
    /// Реализация сервиса Profile.
    /// </summary>
    public class ProfileService : IProfileService
    {
        private readonly ProfileContext _profileContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="profileContext">контекст.</param>
        /// <param name="mapper">профиль AutoMapper.</param>
        public ProfileService(ProfileContext profileContext, IMapper mapper)
        {
            _profileContext = profileContext;
            _mapper = mapper;
        }

        /// <inheritdoc/>
        public async Task<(string result, bool success)> AddNewProfileAsync(ProfileViewModel profileViewModel)
        {
            var profile = _mapper.Map<Models.Profile>(profileViewModel);

			if (await _profileContext.Profiles.FirstOrDefaultAsync(p=>p.Id == profileViewModel.Id) != null)
			{
				///TODO: Вынести результат в константы
				return ("fail", false);
			}

            await _profileContext.Profiles.AddAsync(profile);
            await _profileContext.SaveChangesAsync();

            var id = profile.Id.ToString();

            return (id, true);
        }

        /// <inheritdoc/>
        public async Task<List<ProfileViewModel>> GetAllProfilesAsync(int? take = null, int? skip = null)
        {
            var queriableListOfProfiles = _profileContext.Profiles.Select(x => x);

            if (take != null && take > 0 && skip != null && skip > 0)
            {
                queriableListOfProfiles = queriableListOfProfiles.Skip((int)skip).Take((int)take);
            }

            var listOfProfiles = await queriableListOfProfiles.ToListAsync();

            var listOfProfilesViewModels = new List<ProfileViewModel>();

            foreach (var profileModel in listOfProfiles)
            {
                var profileViewModel = _mapper.Map<ProfileViewModel>(profileModel);
                listOfProfilesViewModels.Add(profileViewModel);
            }

            return listOfProfilesViewModels;
        }

        /// <inheritdoc/>
        public async Task<ProfileViewModel> GetProfileByIdAsync(Guid profileId)
        {
            var profile = await _profileContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileId);
            var profileViewModel = _mapper.Map<ProfileViewModel>(profile);

            return profileViewModel;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateProfileAsync(ProfileViewModel profileViewModel)
        {
            var profile = await _profileContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileViewModel.Id);

            if (profile == null)
            {
                return false;
            }

            profile.FirstName = profileViewModel.FirstName;
            profile.LastName = profileViewModel.LastName;
            profile.SecondName = profileViewModel.SecondName;
            profile.IsSeller = profileViewModel.IsSeller;
            profile.OrgName = profileViewModel.OrgName;
            profile.OrgNumber = profileViewModel.OrgNumber;
            profile.BankBook = profileViewModel.BankBook;
            profile.Balance = profileViewModel.Balance;

            _profileContext.Update(profile);
            await _profileContext.SaveChangesAsync();

            return true;
        }
    }
}
