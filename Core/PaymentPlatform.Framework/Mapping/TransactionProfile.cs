using AutoMapper;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для Transaction.
    /// </summary>
    public class TransactionProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public TransactionProfile()
        {
            CreateMap<TransactionViewModel, TransactionModel>().ReverseMap();
            CreateMap<TransactionViewModel, ProductReservedModel>().ReverseMap();
            CreateMap<TransactionViewModel, BalanceReservedModel>().ReverseMap();
        }
    }
}
