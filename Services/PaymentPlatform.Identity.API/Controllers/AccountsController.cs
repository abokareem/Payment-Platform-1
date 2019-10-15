using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentPlatform.Framework.Constants;
using PaymentPlatform.Framework.Constants.Logger;
using PaymentPlatform.Framework.ViewModels;
using PaymentPlatform.Identity.API.Services.Interfaces;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentPlatform.Identity.API.Controllers
{
	/// <summary>
	/// Основной контроллер для Identity.
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : Controller
	{
		private readonly IAccountService _accountService;

        /// <summary>
        /// Конструктор с параметрами.
        /// </summary>
        /// <param name="accountService">account сервис.</param>
        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/accounts
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IEnumerable<AccountViewModel>> GetAccounts(int? take, int? skip)
        {
            var accounts = await _accountService.GetAllAccountsAsync(take, skip);
            var count = accounts.Count;

            Log.Information($"{count} {IdentityLoggerConstants.ACCOUNT_RECEIVED}");

            return await _accountService.GetAllAccountsAsync(take, skip);
        }

        // GET: api/accounts/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProfile([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = await _accountService.GetAccountByIdAsync(id);

            if (account == null)
            {
                Log.Warning($"{account.Login} {IdentityLoggerConstants.ACCOUNT_NOT_FOUND}");

                return NotFound();
            }

            Log.Information($"{account.Login} {IdentityLoggerConstants.ACCOUNT_RECEIVED}");

            return Ok(account);
        }

        // POST: api/accounts/auth
        [AllowAnonymous]
		[HttpPost("auth")]
		public async Task<IActionResult> Authenticate([FromBody] LoginViewModel data)
		{
            if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = await _accountService.AuthenticateAsync(data);

			if (token == null)
			{
                Log.Warning($"{data.Email} {IdentityLoggerConstants.EMAIL_NOT_FOUND}");

                return BadRequest(IdentityConstants.USER_DATA_INCORRECT);
			}

            Log.Information($"{data.Email} {IdentityLoggerConstants.EMAIL_FOUND}");

            Response.ContentType = "application/json";
			return Accepted(token);
		}

        // POST: api/accounts/registration
        [AllowAnonymous]
		[HttpPost("registration")]
		public async Task<IActionResult> Registration([FromBody] AccountViewModel account)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var (successfullyRegistered, message) = await _accountService.RegistrationAsync(account);

			if (!successfullyRegistered)
			{
                Log.Warning($"{account.Email} {IdentityLoggerConstants.EMAIL_EXIST}");

                return BadRequest(new { message });
			}

            Log.Information($"{account.Email} {IdentityLoggerConstants.EMAIL_REGISTRATION_SUCCESS}");

            return Ok(new { message });
		}

        // PUT: api/accounts/{id}
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount([FromBody] AccountViewModel account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var accountViewModel = await _accountService.GetAccountByEmailAsync(account.Email);
            var accountExist = accountViewModel != null;

            if (!accountExist)
            {
                Log.Warning($"{account.Email} {IdentityLoggerConstants.EMAIL_NOT_EXIST}");

                return NotFound();
            }

            var updatedResult = await _accountService.UpdateAccountAsync(account);

            if (!updatedResult)
            {
                Log.Warning($"{account.Email} {IdentityLoggerConstants.EMAIL_UPDATE_NOT_SUCCESS}");

                return Conflict();
            }

            Log.Information($"{account.Email} {IdentityLoggerConstants.EMAIL_UPDATE_SUCCESS}");

            return Ok(account);
        }
    }
}
