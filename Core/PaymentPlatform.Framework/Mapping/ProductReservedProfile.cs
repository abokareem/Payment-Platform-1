using AutoMapper;
using PaymentPlatform.Framework.Models;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
    /// Профиль AutoMapper для ProductReserved.
    /// </summary>
    public class ProductReservedProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public ProductReservedProfile()
        {
            CreateMap<ProductReservedModel, TransactionModel>().ReverseMap();
        }
    }
}