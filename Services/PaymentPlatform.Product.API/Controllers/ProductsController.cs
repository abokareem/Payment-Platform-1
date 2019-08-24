using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Product.API.Services.Interfaces;
using PaymentPlatform.Product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

		// GET: api/Products
		[HttpGet]
		public async Task<IEnumerable<ProductViewModel>> GetProducts(int? take, int? skip)
		{
			return await _productService.GetAllProductsAsyc(take, skip);
		}

		// GET: api/Products/5
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

		// PUT: api/Products/5
		[Authorize(Roles = "User, Admin")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct([FromBody] ProductViewModel product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.Identities.First().Claims.FirstOrDefault(c => c.Type == "id").Value;
			var userRole = User.Identities.First().Claims.FirstOrDefault(c => c.Type.Contains("role")).Value;
			var productExists = await ProductExists(product.Id, new Guid(userId), userRole);

			if (!productExists)
			{
				return Forbid();
			}

			var successfullyUpdated = await _productService.UpdateProductAsync(product);

			if (successfullyUpdated)
			{
				return Ok(product);
			}
			else
			{
				return Conflict();
			}
		}

		// POST: api/Products
		[Authorize(Roles = "User, Admin")]
		[HttpPost]
		public async Task<IActionResult> PostProduct([FromBody] ProductViewModel product)
		{
			var userId = new Guid(User.Identities.First().Claims.FirstOrDefault(c => c.Type == "id").Value);
			var userRole = User.Identities.First().Claims.FirstOrDefault(c => c.Type.Contains("role")).Value;

			if (!ModelState.IsValid || (product.ProfileId != userId && userRole != "Admin"))
			{
				return BadRequest(ModelState);
			}

			var id = await _productService.AddNewProductAsync(product, new UserViewModel { Id = userId, Role = userRole });
			if (id != null)
			{
				return CreatedAtAction(nameof(PostProduct), product);
			}
			else
			{
				return Conflict();
			}

		}

		// DELETE: api/Products/5
		[Authorize(Roles = "User, Admin")]
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var userId = User.Identities.First().Claims.FirstOrDefault(c => c.Type == "id").Value;
			var userRole = User.Identities.First().Claims.FirstOrDefault(c => c.Type.Contains("role")).Value;
			var productExists = await ProductExists(id, new Guid(userId), userRole);

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
			else
			{
				return NotFound();
			}
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
			else
			{
				return product != null && product.ProfileId == userId;
			}
		}
	}
}