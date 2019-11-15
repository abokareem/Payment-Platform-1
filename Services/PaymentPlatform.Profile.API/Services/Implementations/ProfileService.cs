using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentPlatform.Framework.Constants;
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

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="profileContext">контекст.</param>
        /// <param name="mapper">профиль AutoMapper.</param>
        /// <param name="rabbitService">Сервис брокера сообщений.</param>
        public ProfileService(ProfileContext profileContext,
                              IMapper mapper,
                              IRabbitMQService rabbitService)
        {
            _profileContext = profileContext ?? throw new ArgumentException(nameof(profileContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _rabbitService = rabbitService ?? throw new ArgumentException(nameof(rabbitService));

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
                var incomingObject = JsonConvert.DeserializeObject(incomingMessage) as RabbitMessageModel;

                switch (incomingObject.Sender)
                {
                    case "TransactionAPI":
                        {
                            var balanceReserve = incomingObject.Model as BalanceReservedModel;
                            var profile = _profileContext.Profiles.FirstOrDefaultAsync(p => p.Id == balanceReserve.ProfileId).GetAwaiter().GetResult();
                            if (incomingObject.Action == (int)RabbitMessageActions.Apply)
                            {
                                if (profile != null && profile.Balance >= balanceReserve.Total)
                                {
                                    profile.Balance -= balanceReserve.Total;
                                    balanceReserve.Status = (int)ProductReserveStatus.Peserved;

                                    _profileContext.Entry(profile).State = EntityState.Modified;
                                    _profileContext.Entry(balanceReserve).State = EntityState.Added;

                                    _profileContext.SaveChangesAsync().GetAwaiter().GetResult();

                                    _rabbitService.SendMessage(JsonConvert.SerializeObject(new RabbitMessageModel { Action = (int)RabbitMessageActions.Apply, Sender = "ProductAPI", Model = balanceReserve }), "TransactionAPI");
                                }
                            }
                            else if (incomingObject.Action == (int)RabbitMessageActions.Revert)
                            {
                                if (profile != null)
                                {
                                    profile.Balance += balanceReserve.Total;
                                    balanceReserve.Status = (int)ProductReserveStatus.NotReserved;

                                    _profileContext.Entry(profile).State = EntityState.Modified;
                                    _profileContext.Entry(balanceReserve).State = EntityState.Modified;

                                    _profileContext.SaveChangesAsync().GetAwaiter().GetResult();

                                    _rabbitService.SendMessage(JsonConvert.SerializeObject(new RabbitMessageModel { Action = (int)RabbitMessageActions.Revert, Sender = "ProductAPI", Model = balanceReserve }), "TransactionAPI");
                                }
                            }
                            else
                            {
                                throw new JsonException("Unexpected action.");
                            }
                            break;
                        }
                    default:
                        throw new JsonException("Unexpected sender.");
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