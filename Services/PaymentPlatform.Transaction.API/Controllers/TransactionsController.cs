using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Controllers
{
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class TransactionsController : ControllerBase
	{
		private readonly ITransactionService _transactionService;

		public TransactionsController(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}

		// GET: api/Transactions
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IEnumerable<TransactionViewModel>> GetTransactions(int? take = null, int? skip = null)
		{
			return await _transactionService.GetTransactionsAsync(take,skip);
		}

		// GET: api/Transactions/5
		[Authorize(Roles = "Admin, User")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var transaction = await _transactionService.GetTransactionByIdAsync(id);

			if (transaction == null)
			{
				return NotFound();
			}

			return Ok(transaction);
		}

		// PUT: api/Transactions/5
		[Authorize(Roles = "Admin, User")]
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTransaction([FromBody] TransactionViewModel transaction)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			await _transactionService.UpdateTransactionAsync(transaction);
			
			return NoContent();
		}

		// POST: api/Transactions
		[HttpPost]
		public async Task<IActionResult> PostTransaction([FromBody] TransactionViewModel transaction)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _transactionService.AddNewTransactionAsync(transaction);
			if (result.success)
			{
				return Ok(transaction);
			}
			else
			{
				return BadRequest(result.message);
			}
		}

		// DELETE: api/Transactions/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (!TransactionExists(id))
			{
				return BadRequest();
			}
			var result = await _transactionService.RevertTransactionByIdAsync(id);
			if (result.success)
			{
				return Ok(result.message);
			}
			else
			{
				return Conflict(result.message);
			}
		}

		private bool TransactionExists(Guid id)
		{
			return _transactionService.GetTransactionByIdAsync(id) != null;
		}

		private (Guid, string) GetClaimsIdentity()
		{
			var id = User.Identity.Name;

			var userIdentity = (ClaimsIdentity)User.Identity;
			var claims = userIdentity.Claims;
			var roleClaimType = userIdentity.RoleClaimType;
			var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

			return (new Guid(id), role);
		}
	}
}