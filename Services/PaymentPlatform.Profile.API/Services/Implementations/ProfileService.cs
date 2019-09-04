using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Profile.API.Models;
using PaymentPlatform.Profile.API.Services.Interfaces;
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
		public ProfileService(ProfileContext profileContext, IMapper mapper, IRabbitMQService rabbitService)
		{
			_profileContext = profileContext;
			_mapper = mapper;
			_rabbitService = rabbitService;
			_rabbitService.SetListener("ProfileAPI", OnIncomingMessage);
		}

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
							var profile = _profileContext.Profiles.FirstOrDefault(p => p.Id == balanceReserve.ProfileId);
							if (incomingObject.Action == "Apply")
							{
								if (profile != null && profile.Balance >= balanceReserve.Total)
								{
									profile.Balance -= balanceReserve.Total;
									//TODO: Реализовать статусы резерва
									balanceReserve.Status = 1;
									_profileContext.Entry(profile).State = EntityState.Modified;
									_profileContext.Entry(balanceReserve).State = EntityState.Added;
									_profileContext.SaveChanges();
									_rabbitService.SendMessage(JsonConvert.SerializeObject(new RabbitMessageModel { Action = "Apply", Sender = "ProductAPI", Model = balanceReserve }), "TransactionAPI");
								}
							}
							else if (incomingObject.Action == "Revert")
							{
								if (profile != null)
								{
									profile.Balance += balanceReserve.Total;
									//TODO: Реализовать статусы резерва
									balanceReserve.Status = 0;
									_profileContext.Entry(profile).State = EntityState.Modified;
									_profileContext.Entry(balanceReserve).State = EntityState.Modified;
									_profileContext.SaveChanges();
									_rabbitService.SendMessage(JsonConvert.SerializeObject(new RabbitMessageModel { Action = "Revert", Sender = "ProductAPI", Model = balanceReserve }), "TransactionAPI");
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
			catch (JsonException jsonExc)
			{
				//TODO: Вывести в лог
			}
			catch (Exception exc)
			{
				throw exc;
			}
		}

		/// <inheritdoc/>
		public async Task<(string result, bool success)> AddNewProfileAsync(ProfileViewModel profileViewModel)
		{
			var profile = _mapper.Map<ProfileModel>(profileViewModel);

			if (await _profileContext.Profiles.FirstOrDefaultAsync(p => p.Id == profileViewModel.Id) != null)
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
