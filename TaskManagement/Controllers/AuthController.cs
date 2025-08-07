using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("signin")]
        public async Task<IActionResult> Signin(Signin value)
        {
            var result = await _authService.SignInAsync(value);

            if (string.IsNullOrEmpty(result))
            {
                return Unauthorized("Username or password is incorrect.");
            }

            return Ok(result);
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(CreateAccount value)
        {
            var result = await _authService.SignUpAsync(value);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.User);
        }
    }
}
