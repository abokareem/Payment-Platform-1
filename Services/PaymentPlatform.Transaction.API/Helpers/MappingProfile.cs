using PaymentPlatform.Transaction.API.ViewModels;
using PaymentPlatform.Transaction.API.Models;

namespace PaymentPlatform.Transaction.API.Helpers
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
            CreateMap<TransactionViewModel, Models.Transaction>().ReverseMap();
			CreateMap<TransactionViewModel, ProductReserve>().ReverseMap();
			CreateMap<TransactionViewModel, BalanceReserve>().ReverseMap();

		}
	}
}
