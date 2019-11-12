using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Framework.Constants.Logger;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Product.API.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="productService">product сервис.</param>
        public ProductsController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentException(nameof(productService));
        }

        // GET: api/products
        [HttpGet]
        public async Task<IEnumerable<ProductViewModel>> GetAllProducts(int? take, int? skip)
        {
            var (userId, _) = GetClaimsIdentity();
            var products = await _productService.GetAllProductsAsync(true, userId, take, skip);
            var count = products.Count;

            Log.Information($"{count} {ProductLoggerConstants.GET_PRODUCTS_ALL}");

            return products;
        }

        // GET: api/products/user
        [HttpGet("user")]
        public async Task<IEnumerable<ProductViewModel>> GetUsersProducts(int? take, int? skip)
        {
            var (userId, userRole) = GetClaimsIdentity();
            var isAdmin = false;

            if (userRole == "Admin")
            {
                isAdmin = true;
            }

            var products = await _productService.GetAllProductsAsync(isAdmin, userId, take, skip);
            var count = products.Count;

            Log.Information($"{count} {ProductLoggerConstants.GET_PRODUCTS_USERS}");

            return products;
        }

        // GET: api/products/user/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IEnumerable<ProductViewModel>> GetProductsByUserId([FromRoute] Guid id, int? take, int? skip)
        {
            var products = await _productService.GetAllProductsAsync(false, id, take, skip);
            var count = products.Count;

            Log.Information($"{count} {ProductLoggerConstants.GET_PRODUCTS_BY_USER_ID}.");

            return products;
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                Log.Warning($"{id} {ProductLoggerConstants.GET_PRODUCT_NOT_FOUND}");

                return NotFound();
            }

            Log.Information($"{id} {ProductLoggerConstants.GET_PRODUCT_FOUND}");

            return Ok(product);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct([FromBody] ProductViewModel product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (userId, userRole) = GetClaimsIdentity();

            var productExists = await ProductExists(product.Id, userId, userRole);

            if (!productExists)
            {
                Log.Warning($"{product.Id} {ProductLoggerConstants.GET_PRODUCT_NOT_FOUND}");

                return Forbid();
            }

            var successfullyUpdated = await _productService.UpdateProductAsync(product);

            if (!successfullyUpdated)
            {
                Log.Warning($"{product.Id} {ProductLoggerConstants.UPDATE_PRODUCT_CONFLICT}");

                return Conflict();
            }

            Log.Information($"{product.Id} {ProductLoggerConstants.UPDATE_PRODUCT_OK}");

            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> AddNewProduct([FromBody] ProductViewModel product)
        {
            var (userId, userRole) = GetClaimsIdentity();

            if (!ModelState.IsValid || (product.ProfileId != userId && userRole != "Admin"))
            {
                return BadRequest(ModelState);
            }

            var id = await _productService.AddNewProductAsync(product);

            if (id == null)
            {
                Log.Warning($"{id} {ProductLoggerConstants.ADD_PRODUCT_CONFLICT}");

                return Conflict();
            }

            Log.Information($"{product.Id} {ProductLoggerConstants.ADD_PRODUCT_OK}");

            return CreatedAtAction(nameof(AddNewProduct), product);
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (userId, userRole) = GetClaimsIdentity();

            var productExists = await ProductExists(id, userId, userRole);

            if (!productExists)
            {
                Log.Warning($"{id} {ProductLoggerConstants.GET_PRODUCT_NOT_FOUND}");

                return NotFound();
            }

            var product = await _productService.GetProductByIdAsync(id);
            product.IsActive = false;

            var successfullyUpdated = await _productService.UpdateProductAsync(product);

            if (!successfullyUpdated)
            {
                Log.Warning($"{id} {ProductLoggerConstants.DELETE_PRODUCT_CONFLICT}");

                return BadRequest();
            }

            Log.Information($"{id} {ProductLoggerConstants.DELETE_PRODUCT_OK}");

            return Ok();
        }

        private async Task<bool> ProductExists(Guid productId, Guid userId, string userRole)
        {
            var product = await _productService.GetProductByIdAsync(productId);

            if (product == null)
            {
                return false;
            }

            if (userRole == "Admin")
            {
                return true;
            }

            return product != null && product.ProfileId == userId;
        }

        private (Guid, string) GetClaimsIdentity()
        {
            var id = User.Identity.Name;

            var userIdentity = (ClaimsIdentity)User.Identity;
            var claims = userIdentity.Claims;
            var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

            return (new Guid(id), role);
        }
    }
}