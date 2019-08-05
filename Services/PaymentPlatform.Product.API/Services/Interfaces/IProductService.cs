using PaymentPlatform.Product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Services.Interfaces
{
	public interface IProductService
	{
		/// <summary>
		/// Добавить новый товар.
		/// </summary>
		/// <param name="productViewModel">Модель продукта.</param>
		/// <param name="userViewModel">Пользователь.</param>
		/// <returns>Id продукта.</returns>
		Task<string> AddNewProductAsync(ProductViewModel productViewModel, UserViewModel userViewModel);
		/// <summary>
		/// Получить товар по его Id.
		/// </summary>
		/// <param name="productId">Id продукта.</param>
		/// <returns></returns>
		Task<ProductViewModel> GetProductByIdAsync(Guid productId);
		/// <summary>
		/// Возвращает все товары пользователя.
		/// </summary>
		/// <param name="userViewModel">Пользователь.</param>
		/// <param name="take">Параметр пагинации. Сколько товаров взять.</param>
		/// <param name="skip">Параметр пагинации. Сколько товаров пропустить.</param>
		/// <returns>Список товаров пользователя.</returns>
		Task<List<ProductViewModel>> GetProductsByUserIdAsyc(UserViewModel userViewModel, int? take = null, int? skip = null);
		/// <summary>
		/// Получить все товары.
		/// </summary>
		/// <param name="take">Параметр пагинации. Сколько товаров взять.</param>
		/// <param name="skip">Параметр пагинации. Сколько товаров пропустить.</param>
		/// <returns>Список товаров.</returns>
		Task<List<ProductViewModel>> GetAllProductsAsyc(int? take = null, int? skip = null);
		/// <summary>
		/// Обновить свойства товара.
		/// </summary>
		/// <param name="productViewModel">Товар.</param>
		/// <returns></returns>
		Task<bool> UpdateProductAsync(ProductViewModel productViewModel);
	}
}
