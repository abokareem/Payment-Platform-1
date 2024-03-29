﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Framework.Constants.Logger;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Transaction.API.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentPlatform.Transaction.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="transactionService">Transaction сервис.</param>
        public TransactionsController(ITransactionService transactionService) => _transactionService = transactionService ?? throw new ArgumentException(nameof(transactionService));

        // GET: api/Transactions
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<TransactionViewModel>> GetTransactions(int? take = null, int? skip = null)
        {
            var transactions = await _transactionService.GetTransactionsAsync(take, skip);
            var count = transactions.ToList().Count;

            Log.Information($"{count} {TransactionLoggerConstants.GET_TRANSACTIONS}");

            return transactions;
        }

        // GET: api/Transactions/{id}
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
                Log.Warning($"{id} {TransactionLoggerConstants.GET_TRANSACTION_NOT_FOUND}");

                return NotFound();
            }

            Log.Information($"{id} {TransactionLoggerConstants.GET_TRANSACTION_FOUND}");

            return Ok(transaction);
        }

        // PUT: api/Transactions/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction([FromBody] TransactionViewModel transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transactionViewModel = await _transactionService.GetTransactionByIdAsync(transaction.Id);
            var transactionExist = transactionViewModel != null;

            if (!transactionExist)
            {
                Log.Warning($"{transaction.Id} {TransactionLoggerConstants.GET_TRANSACTION_NOT_FOUND}");

                return NotFound();
            }

            var updatedResult = await _transactionService.UpdateTransactionAsync(transaction);

            if (!updatedResult)
            {
                Log.Warning($"{transaction.Id} {TransactionLoggerConstants.UPDATE_TRANSACTION_CONFLICT}");

                return Conflict();
            }

            Log.Information($"{transaction.Id} {TransactionLoggerConstants.UPDATE_TRANSACTION_OK}");

            return Ok(transaction);
        }

        // POST: api/Transactions
        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> AddNewTransaction([FromBody] TransactionViewModel transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var (success, guid, message) = await _transactionService.AddNewTransactionAsync(transaction);

            if (!success)
            {
                Log.Warning($"{guid} {TransactionLoggerConstants.ADD_TRANSACTION_CONFLICT}");

                return Conflict(message);
            }

            Log.Information($"{guid} {TransactionLoggerConstants.ADD_TRANSACTION_OK}");

            return Ok(message);
        }

        // DELETE: api/Transactions/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await TransactionExistsAndIsActive(id);

            if (!result)
            {
                Log.Warning($"{id} {TransactionLoggerConstants.GET_TRANSACTION_NOT_FOUND}");

                return NotFound();
            }

            var (success, message) = await _transactionService.RevertTransactionByIdAsync(id);

            if (!success)
            {
                Log.Warning($"{id} {TransactionLoggerConstants.REVERT_TRANSACTION_CONFLICT}");

                return Conflict(message);
            }

            Log.Information($"{id} {TransactionLoggerConstants.REVERT_TRANSACTION_OK}");

            return Ok(message);
        }

        private async Task<bool> TransactionExistsAndIsActive(Guid id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);

            if (transaction.IsActive)
            {
                return true;
            }

            return false;
        }
    }
}