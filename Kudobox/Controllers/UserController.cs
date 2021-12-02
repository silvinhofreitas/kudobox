using System.Threading.Tasks;
using Kudobox.Contexts;
using Kudobox.Dto.Shared;
using Kudobox.Dto.User;
using Kudobox.Helpers.Extensions;
using Kudobox.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kudobox.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserContext _userContext;

        public UserController(UserContext userContext)
        {
            _userContext = userContext;
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginDto input)
        {
            var user = await _userContext.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(input.Username) && u.Password.Equals(input.Password.Encrypt()));

            if (user == null)
                return Unauthorized(new MessageDto("Invalid username or password."));

            var token = TokenService.GenerateToken(user);
      
            return Ok(new
            {
                type = "Bearer",
                token
            });
        }
    }
}