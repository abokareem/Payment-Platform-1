using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.DTO;
using PaymentPlatform.Framework.Enums;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Profile.API.Models;
using PaymentPlatform.Profile.API.Services.Interfaces;
using Serilog;
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
        private readonly IRabbitMQService _rabbitService;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="profileContext">контекст.</param>
        /// <param name="mapper">профиль AutoMapper.</param>
        /// <param name="rabbitService">Сервис брокера сообщений.</param>
        /// /// <param name="scopeFactory">Фабрика для создания объектов IServiceScope.</param>
        public ProfileService(ProfileContext profileContext,
                              IMapper mapper,
                              IRabbitMQService rabbitService,
                              IServiceScopeFactory scopeFactory)
        {
            _profileContext = profileContext ?? throw new ArgumentException(nameof(profileContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _rabbitService = rabbitService ?? throw new ArgumentException(nameof(rabbitService));
            _scopeFactory = scopeFactory ?? throw new ArgumentException(nameof(scopeFactory));

            _rabbitService.ConfigureServiceDefault();
            _rabbitService.SetListener("ProfileAPI", OnIncomingMessage);
        }

        /// <summary>
        /// Метод, вызываемый при получении сообщения от брокера.
        /// </summary>
        /// <param name="incomingMessage">Текст сообщения.</param>
        private void OnIncomingMessage(string incomingMessage)
        {
            try
            {
                var incomingObject = JsonConvert.DeserializeObject<RabbitMessageModel>(incomingMessage);

                if (incomingObject.Sender != "TransactionAPI")
                {
                    throw new JsonException("Unexpected action.");
                }

                var transactionDTO = JsonConvert.DeserializeObject<TransactionDataTransferObject>(incomingObject.Model.ToString());

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProfileContext>();

                    var profile = dbContext.Profiles.FirstOrDefaultAsync(p => p.Id == transactionDTO.ProfileId).GetAwaiter().GetResult();

                    if (profile != null)
                    {
                        // UNDONE: При развитии решения продумать более детальную и улучшеную реализацию
                        switch (incomingObject.Action)
                        {
                            case (int)RabbitMessageActions.Apply:
                                {
                                    profile.Balance -= transactionDTO.Cost;
                                }
                                break;

                            case (int)RabbitMessageActions.Revert:
                                {
                                    profile.Balance += transactionDTO.Cost;
                                }
                                break;

                            default: throw new JsonException("Unexpected action.");
                        }

                        dbContext.Update(profile);
                        dbContext.SaveChangesAsync().GetAwaiter().GetResult();
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                Log.Error(jsonEx, jsonEx.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task<(string result, bool success)> AddNewProfileAsync(ProfileViewModel profileViewModel)
        {
            var profile = _mapper.Map<ProfileModel>(profileViewModel);
            var foundedProfile = await _profileContext.Profiles.FirstOrDefaultAsync(p => p.Passport == profileViewModel.Passport);

            if (foundedProfile != null)
            {
                return (GlobalConstants.PROFILE_SERVICE_FAIL, false);
            }

            await _profileContext.Profiles.AddAsync(profile);
            await _profileContext.SaveChangesAsync();

            var id = profile.Id.ToString();

            return (id, true);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProfileViewModel>> GetAllProfilesAsync(int? take = null, int? skip = null)
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