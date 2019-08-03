using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Interfaces;

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
        public IActionResult Authenticate([FromBody] Login data)
        {
            var token = _accountService.Authenticate(data.Email, data.Password);

            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(token);
        }
    }  
}
