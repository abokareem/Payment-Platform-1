using AutoMapper;
using PaymentPlatform.Product.API.ViewModels;

namespace PaymentPlatform.Product.API.Helpers
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
			CreateMap<ProductViewModel, Product>().ReverseMap();
		}
	}
}
