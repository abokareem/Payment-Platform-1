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

        /// <summary>
        /// Аутентификация.
        /// </summary>
        /// <param name="data">данные.</param>
        /// <returns>Результат получения JWT токена.</returns>
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
                return BadRequest(new { message = AppConstants.USER_DATA_INCORRECT });
            }

            return Ok(token);
        }

        /// <summary>
        /// Аутентификация.
        /// </summary>
        /// <param name="data">данные.</param>
        /// <returns>Результат получения JWT токена.</returns>
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] AccountViewModel account)
        {
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var (result, message) = await _accountService.RegistrationAsync(account);

            if (!result)
            {
                return BadRequest(new { message = message });
            }

            return Ok(); 
        }
    }  
}
