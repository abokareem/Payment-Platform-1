using AutoMapper;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.ViewModels;

namespace PaymentPlatform.Framework.Mapping
{
    /// <summary>
	/// Профиль AutoMapper для Product.
	/// </summary>
	public class ProductProfile : Profile
    {
        /// <summary>
        /// Пустой конструктор.
        /// </summary>
        public ProductProfile()
        {
            CreateMap<ProductViewModel, AppProduct>().ReverseMap();
        }
    }
}
