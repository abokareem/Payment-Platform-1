using AutoMapper;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для Profile.
    /// </summary>
    public class UserProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public UserProfile()
        {
            CreateMap<ProfileViewModel, ProfileModel>().ReverseMap();
        }
    }
}