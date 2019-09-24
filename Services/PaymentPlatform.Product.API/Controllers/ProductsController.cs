using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Product.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Controllers
{
    /// <summary>
    /// Основной контроллер для Product.
    /// </summary>
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
			_productService = productService;
		}

		// GET: api/products
		[HttpGet]
		public async Task<IEnumerable<ProductViewModel>> GetAllProducts(int? take, int? skip)
		{
            var (userId, userRole) = GetClaimsIdentity();

            return await _productService.GetAllProductsAsyc(true, userId, take, skip);
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

            return await _productService.GetAllProductsAsyc(isAdmin, userId, take, skip);
        }

        // GET: api/products/user/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("user/{id}")]
        public async Task<IEnumerable<ProductViewModel>> GetProductsByUserId([FromRoute] Guid id, int? take, int? skip)
        {
            return await _productService.GetAllProductsAsyc(false , id, take, skip);
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
				return NotFound();
			}

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
				return Forbid();
			}

			var successfullyUpdated = await _productService.UpdateProductAsync(product);

			if (successfullyUpdated)
			{
				return Ok(product);
			}

            return Conflict();
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

			var id = await _productService.AddNewProductAsync(product, new UserViewModel { Id = userId, Role = userRole });

			if (id != null)
			{
				return CreatedAtAction(nameof(AddNewProduct), product);
			}

            return Conflict();
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

			if (productExists)
			{
				var product = await _productService.GetProductByIdAsync(id);
				product.IsActive = false;

				var successfullyUpdated = await _productService.UpdateProductAsync(product);

				if (successfullyUpdated)
				{
					return Ok();
				}

				return BadRequest();
			}

            return NotFound();
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