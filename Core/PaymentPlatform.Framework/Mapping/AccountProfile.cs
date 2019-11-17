using AutoMapper;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для Identity.
    /// </summary>
    public class AccountProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public AccountProfile()
        {
            CreateMap<LoginViewModel, LoginModel>().ReverseMap();

            CreateMap<AccountViewModel, AccountModel>().ReverseMap();
        }
    }
}