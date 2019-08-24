using AutoMapper;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Interfaces;
using PaymentPlatform.Product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace PaymentPlatform.Product.API.Services.Implementations
{
    /// <summary>
    /// Реализация сервиса Product.
    /// </summary>
	public class ProductService : IProductService
	{
		private readonly ProductContext _productContext;
		private readonly IMapper _mapper;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="productContext">контекст.</param>
        /// <param name="mapper">профиль AutoMapper.</param>
		public ProductService(ProductContext productContext, IMapper mapper)
		{
			_productContext = productContext;
			_mapper = mapper;
		}

        /// <inheritdoc/>
		public async Task<string> AddNewProductAsync(ProductViewModel productViewModel, UserViewModel userViewModel)
		{
			var product = _mapper.Map<Models.Product>(productViewModel);

			await _productContext.Products.AddAsync(product);
			await _productContext.SaveChangesAsync();

			var id = product.Id.ToString();

			return id;
		}

        /// <inheritdoc/>
		public async Task<List<ProductViewModel>> GetAllProductsAsyc(bool isAdmin, Guid profileId, int? take = null, int? skip = null)
		{
			IQueryable<Models.Product> queriableListOfProducts = null;

            if (isAdmin)
            {
                queriableListOfProducts = _productContext.Products.Select(x => x);
            }
            else
            {
                queriableListOfProducts = _productContext.Products.Select(x => x).Where(p => p.ProfileId == profileId);
            }

			if (take != null && take > 0 && skip != null && skip > 0)
			{
				queriableListOfProducts = queriableListOfProducts.Skip((int)skip).Take((int)take);
			}

			var listOfProducts = await queriableListOfProducts.ToListAsync();
			var listOfViewModels = new List<ProductViewModel>();

			foreach (var productModel in listOfProducts)
			{
				var productViewModel = _mapper.Map<ProductViewModel>(productModel);
				listOfViewModels.Add(productViewModel);
			}

			return listOfViewModels;
		}

        /// <inheritdoc/>
		public async Task<ProductViewModel> GetProductByIdAsync(Guid productId)
		{
			var product = await _productContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
			var productViewModel = _mapper.Map<ProductViewModel>(product);

			return productViewModel;
		}

        /// <inheritdoc/>
		public async Task<List<ProductViewModel>> GetProductsByUserIdAsyc(UserViewModel userViewModel, int? take = null, int? skip = null)
		{
			var listOfProductViewModel = new List<ProductViewModel>();
			var listOfProducts = await _productContext.Products.Where(p => p.ProfileId == userViewModel.Id).ToListAsync();

			foreach (var product in listOfProducts)
			{
				var productViewModel = _mapper.Map<ProductViewModel>(product);
				listOfProductViewModel.Add(productViewModel);
			}

			return listOfProductViewModel;
		}

        /// <inheritdoc/>
		public async Task<bool> UpdateProductAsync(ProductViewModel productViewModel)
		{
			var product = await _productContext.Products.FirstOrDefaultAsync(p => p.Id == productViewModel.Id);

            if (product == null)
            {
                return false;
            }

            product.Name = productViewModel.Name;
            product.Description = productViewModel.Description;
            product.MeasureUnit = productViewModel.MeasureUnit;
            product.Category = productViewModel.Category;
            product.Amount = productViewModel.Amount;
            product.Price = productViewModel.Price;

            _productContext.Update(product);
            await _productContext.SaveChangesAsync();

            return true;
        }
	}
}
