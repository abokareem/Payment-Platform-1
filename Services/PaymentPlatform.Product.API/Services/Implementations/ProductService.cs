using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using PaymentPlatform.Framework.DTO;
using PaymentPlatform.Framework.Enums;
using PaymentPlatform.Framework.Models;
using PaymentPlatform.Framework.Services.RabbitMQ.Interfaces;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Product.API.Models;
using PaymentPlatform.Product.API.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Services.Implementations
{
    /// <summary>
    /// Реализация сервиса Product.
    /// </summary>
	public class ProductService : IProductService
    {
        private readonly ProductContext _productContext;
        private readonly IMapper _mapper;
        private readonly IRabbitMQService _rabbitService;
        private readonly IServiceScopeFactory _scopeFactory;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="productContext">контекст.</param>
        /// <param name="mapper">профиль AutoMapper.</param>
        /// <param name="rabbitService">Сервис брокера сообщений.</param>
        /// <param name="scopeFactory">Фабрика для создания объектов IServiceScope.</param>
        public ProductService(ProductContext productContext, 
                              IMapper mapper,
                              IRabbitMQService rabbitService,
                              IServiceScopeFactory scopeFactory)
        {
            _productContext = productContext ?? throw new ArgumentException(nameof(productContext));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
            _rabbitService = rabbitService ?? throw new ArgumentException(nameof(rabbitService));
            _scopeFactory = scopeFactory ?? throw new ArgumentException(nameof(scopeFactory));

            _rabbitService.ConfigureServiceDefault();
            _rabbitService.SetListener("ProductAPI", OnIncomingMessage);
        }

        /// <summary>
        /// Метод, вызываемый при получении сообщения от брокера.
        /// </summary>
        /// <param name="incomingMessage">Текст сообщения.</param>
        private void OnIncomingMessage(string incomingMessage)
        {
            try
            {
                var incomingObject = JsonConvert.DeserializeObject<RabbitMessageModel>(incomingMessage);

                if (incomingObject.Sender != "TransactionAPI")
                {
                    throw new JsonException("Unexpected action.");
                }

                var transactionDTO = JsonConvert.DeserializeObject<TransactionDataTransferObject>(incomingObject.Model.ToString());

                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ProductContext>();

                    var product = dbContext.Products.FirstOrDefaultAsync(p => p.Id == transactionDTO.ProductId).GetAwaiter().GetResult();

                    if (product != null && product.IsActive)
                    {
                        // UNDONE: При развитии решения продумать более детальную и улучшеную реализацию
                        switch (incomingObject.Action)
                        {
                            case (int)RabbitMessageActions.Apply:
                                {
                                    //При более продуманной реализации использовать: product.Amount >= 1
                                    product.Amount--;
                                }
                                break;

                            case (int)RabbitMessageActions.Revert:
                                {
                                    product.Amount++;
                                }
                                break;

                            default: throw new JsonException("Unexpected action.");
                        }

                        dbContext.Update(product);
                        dbContext.SaveChangesAsync().GetAwaiter().GetResult();
                    }
                }
            }
            catch (JsonException jsonEx)
            {
                Log.Error(jsonEx, jsonEx.Message);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);

                throw ex;
            }
        }

        /// <inheritdoc/>
        public async Task<string> AddNewProductAsync(ProductViewModel productViewModel)
        {
            var product = _mapper.Map<ProductModel>(productViewModel);

            await _productContext.Products.AddAsync(product);
            await _productContext.SaveChangesAsync();

            var id = product.Id.ToString();

            return id;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<ProductViewModel>> GetAllProductsAsync(bool isAdmin, Guid profileId, int? take = null, int? skip = null)
        {
            IQueryable<ProductModel> queriableListOfProducts = null;

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
        public async Task<IEnumerable<ProductViewModel>> GetProductsByUserIdAsync(Guid profileId, int? take = null, int? skip = null)
        {
            var listOfProductViewModel = new List<ProductViewModel>();
            var listOfProducts = await _productContext.Products.Where(p => p.ProfileId == profileId).ToListAsync();

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
            product.IsActive = productViewModel.IsActive;

            _productContext.Update(product);
            await _productContext.SaveChangesAsync();

            return true;
        }
    }
}