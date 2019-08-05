using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Interfaces;
using PaymentPlatform.Product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Services.Implementations
{
	public class ProductService : IProductService
	{
		private readonly ProductContext _productContext;

		public Task<int> AddNewProductAsync(ProductViewModel productViewModel, UserViewModel userViewModel)
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductViewModel>> GetAllProductsAsyc(int? take = null, int? skip = null)
		{
			throw new NotImplementedException();
		}

		public Task<ProductViewModel> GetProductByIdAsync(Guid productId)
		{
			throw new NotImplementedException();
		}

		public Task<List<ProductViewModel>> GetProductsByUserIdAsyc(UserViewModel userViewModel, int? take = null, int? skip = null)
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateProductAsync(ProductViewModel productViewModel)
		{
			throw new NotImplementedException();
		}
	}
}
