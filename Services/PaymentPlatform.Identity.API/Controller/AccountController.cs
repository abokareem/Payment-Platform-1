using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentPlatform.Identity.API.Models;
using PaymentPlatform.Identity.API.Services.Interfaces;

namespace PaymentPlatform.Identity.API.Controller
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _userService;

        public AccountController(IAccountService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Authenticate([FromBody] Login loginParams)
        {
            var token = _userService.Authenticate(loginParams.Email, loginParams.Password);

            if (token is null)
            {
                return BadRequest(new { message = "Username or password is incorrect" });
            }

            return Ok(token);
        }
    }  
}
