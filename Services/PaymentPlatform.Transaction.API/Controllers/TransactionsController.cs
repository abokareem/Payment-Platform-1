using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaymentPlatform.Transaction.API.Models;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using PaymentPlatform.Transaction.API.ViewModels;
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
		private readonly TransactionContext _context;
		private readonly ITransactionService _transactionService;

		public TransactionsController(ITransactionService transactionService)
		{
			_transactionService = transactionService;
		}

		// GET: api/Transactions
		//[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IEnumerable<TransactionViewModel>> GetTransactions(int? take = null, int? skip = null)
		{
			return await _transactionService.GetTransactionsAsync(take,skip);
		}

		// GET: api/Transactions/5
		//[Authorize(Roles = "Admin, User")]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTransaction([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var transaction = await _context.Transactions.FindAsync(id);

			if (transaction == null)
			{
				return NotFound();
			}

			return Ok(transaction);
		}

		// PUT: api/Transactions/5
		[HttpPut("{id}")]
		public async Task<IActionResult> PutTransaction([FromBody] TransactionViewModel transaction)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Entry(transaction).State = EntityState.Modified;

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

			await _context.SaveChangesAsync();

			return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
		}

		// DELETE: api/Transactions/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var transaction = await _context.Transactions.FindAsync(id);
			if (transaction == null)
			{
				return NotFound();
			}

			_context.Transactions.Remove(transaction);
			await _context.SaveChangesAsync();

			return Ok(transaction);
		}

		private bool TransactionExists(Guid id)
		{
			return _context.Transactions.Any(e => e.Id == id);
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