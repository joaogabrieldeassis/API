using Blog.Services;
using Microsoft.AspNetCore.Mvc;
namespace Blog.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly TokenServices _tokenServices;
        public AccountController(TokenServices tokenServices)
        {
            _tokenServices = tokenServices;
        }
        [HttpPost("v1/login")]
        public IActionResult Login()
        {
            
            var token = _tokenServices.GenerateToken(null);
            return Ok(token);
        }
    }
}
