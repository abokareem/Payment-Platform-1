using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Identity.API.Helpers;
using PaymentPlatform.Identity.API.Services.Interfaces;
using PaymentPlatform.Identity.API.ViewModels;
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
				return BadRequest(AppConstants.USER_DATA_INCORRECT);
			}

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
				return BadRequest(new { message });
			}

			return Ok(new { message });
		}

        // PUT: api/accounts/auth
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
                return NotFound();
            }

            var updatedResult = await _accountService.UpdateAccountAsync(account);

            if (!updatedResult)
            {
                return Conflict();
            }

            return Ok(account);
        }
    }
}
