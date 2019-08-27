using PaymentPlatform.Profile.API.ViewModels;

namespace PaymentPlatform.Profile.API.Helpers
{
    /// <summary>
    /// Профиль AutoMapper.
    /// </summary>
    public class MappingProfile : AutoMapper.Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<ProfileViewModel, Core.Models.DatabaseModels.Profile>().ReverseMap();
        }
    }
}
