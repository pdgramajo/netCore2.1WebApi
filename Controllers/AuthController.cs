using Api.DataProviders;
using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private UserDataProvider _userDataProvider;

        public AuthController(UserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] UserForSingIn userData)
        {
            if (_userDataProvider.CheckUserName(userData))
            {
                var tokenString = _userDataProvider.GenerateJwtToken(userData.Username);
                return Ok(new { token = tokenString });
            }

            return BadRequest(new { Error = "wrong request" });
        }
    }
}