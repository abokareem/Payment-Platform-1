using AutoMapper;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для BalanceReserved.
    /// </summary>
    public class BalanceReservedProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public BalanceReservedProfile() => CreateMap<BalanceReservedModel, TransactionModel>().ReverseMap();
    }
}