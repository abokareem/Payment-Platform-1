using AutoMapper;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.ViewModels;

namespace PaymentPlatform.Identity.API.Helpers
{
    /// <summary>
    /// Профиль AutoMapper.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public MappingProfile()
        {
            CreateMap<LoginViewModel, Login>().ReverseMap();

            CreateMap<AccountViewModel, Account>().ReverseMap();
        }
    }
}
