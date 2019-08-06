using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Product.API.Services.Interfaces;
using PaymentPlatform.Product.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Product.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

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
		[HttpPut("{id}")]
		public async Task<IActionResult> PutProduct([FromBody] ProductViewModel product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!await ProductExists(product.Id))
			{
				return NotFound();
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
		[HttpPost]
		public async Task<IActionResult> PostProduct([FromBody] ProductViewModel product)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var id = await _productService.AddNewProductAsync(product, new UserViewModel());
			if (id != null)
			{
				product.Id = new Guid(id);
				return CreatedAtAction("PostProduct", product);
			}
			else
			{
				return Conflict();
			}

		}

		// DELETE: api/Products/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (await ProductExists(id))
			{
				var product = await _productService.GetProductByIdAsync(id);
				product.IsActive = false;

				var successfullyUpdated = await _productService.UpdateProductAsync(product);

				if (successfullyUpdated)
				{
					return Ok();
				}
			}
			return NotFound();
		}

		private async Task<bool> ProductExists(Guid id)
		{
			var product = await _productService.GetProductByIdAsync(id);
			return product != null ? true : false;
		}
	}
}