using AutoMapper;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper.
    /// </summary>
    public class TransactionProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public TransactionProfile()
        {
            CreateMap<TransactionViewModel, AppTransaction>().ReverseMap();
            CreateMap<TransactionViewModel, AppProductReserved>().ReverseMap();
            CreateMap<TransactionViewModel, AppBalanceReserved>().ReverseMap();
        }
    }
}
