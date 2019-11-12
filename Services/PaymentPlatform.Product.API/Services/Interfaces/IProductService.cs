using PaymentPlatform.Framework.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса товаров.
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// Добавить новый товар.
        /// </summary>
        /// <param name="productViewModel">Модель продукта.</param>
        /// <returns>Id продукта.</returns>
        Task<string> AddNewProductAsync(ProductViewModel productViewModel);

        /// <summary>
        /// Получить товар по его Id.
        /// </summary>
        /// <param name="productId">Id продукта.</param>
        /// <returns>ViewModel продукта.</returns>
        Task<ProductViewModel> GetProductByIdAsync(Guid productId);

        /// <summary>
        /// Возвращает все товары пользователя.
        /// </summary>
        /// <param name="profileId">Id пользователя.</param>
        /// <param name="take">Параметр пагинации (кол-во взять).</param>
        /// <param name="skip">Параметр пагинации (кол-во пропустить).</param>
        /// <returns>Список товаров пользователя.</returns>
        Task<List<ProductViewModel>> GetProductsByUserIdAsync(Guid profileId, int? take = null, int? skip = null);

        /// <summary>
        /// Получить все товары.
        /// </summary>
        /// <param name="take">Параметр пагинации (кол-во взять).</param>
        /// <param name="skip">Параметр пагинации (кол-во пропустить).</param>
        /// <returns>Список товаров.</returns>
        Task<List<ProductViewModel>> GetAllProductsAsync(bool isAdmin, Guid profileId, int? take = null, int? skip = null);

        /// <summary>
        /// Обновить свойства товара.
        /// </summary>
        /// <param name="productViewModel">Товар.</param>
        /// <returns>Результат операции.</returns>
        Task<bool> UpdateProductAsync(ProductViewModel productViewModel);
    }
}
