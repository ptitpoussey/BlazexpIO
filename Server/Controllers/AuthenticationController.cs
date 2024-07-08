using BaseLib.DTOs;
using Microsoft.AspNetCore.Mvc;
using ServerLib.Repositories;

namespace Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController(IUserAccountActions userAccountActions) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(RegisterModel user)
        {
            if (user == null) return BadRequest("User is null.");
            var userCheck = await userAccountActions.CreateAsync(user);
            if (userCheck == null) return BadRequest("Error creating the account");
            return Ok(userCheck);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginModel user)
        {
            if (user == null) return BadRequest("User is null.");
            return Ok(await userAccountActions.SignInAsync(user));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null) return BadRequest("Refresh token is null.");
            return Ok(await userAccountActions.RefreshTokenAsync(refreshToken));
        }
    }
}
