using System;
using System.Linq;
using System.Threading.Tasks;
using Kudobox.Contexts;
using Kudobox.Dto.Shared;
using Kudobox.Dto.User;
using Kudobox.Helpers.Constants;
using Kudobox.Services.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kudobox.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        private readonly IStringLocalizer _translator;

        public UserController(UserContext userContext,
            IStringLocalizerFactory factory)
        {
            _userService = new UserService(userContext);
            _translator = factory.Create("User.User", System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
        }
        
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Authenticate([FromBody] UserLoginDto input)
        {
            var user = await _userService.Login(input);

            if (user == null)
                return Unauthorized(new MessageDto(_translator[UserResourceConstants.InvalidUserOrPassword]));

            var token = TokenService.GenerateToken(user);
      
            return Ok(new
            {
                type = "Bearer",
                token
            });
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ManageUserDto input)
        {
            var (user, tempPassword) = await _userService.CreateNewUser(input);

            if (user == null)
                return BadRequest(new MessageDto(_translator[UserResourceConstants.UsernameAlreadyExists]));
            
            // TODO: implement the service to send tempPassword by email and remove from created path 
            return Created(tempPassword, user);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserList(int page = 1, int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;
            
            var userList = await _userService.GetAllUsers(page, pageSize);
            return Ok(userList);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult> GetUserById(Guid id)
        {
            var user = await _userService.FindUserById(id);

            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            return Ok(user.ToDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteUserById(Guid id)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            await _userService.DeleteUser(user);

            return Ok(new MessageDto(string.Format(_translator[UserResourceConstants.UserDeleted], user.Username)));
        }

        [HttpGet]
        [Route("{id}/roles")]
        public async Task<ActionResult> GetUserRoles(Guid id)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            var roles = _userService.ListUserRoles(user);

            return Ok(roles);
        }

        [HttpPost]
        [Route("{id}/roles")]
        public async Task<ActionResult> SetUserRoles(Guid id, [FromBody] ManageUserRolesDto roles)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            var invalidRoles = await _userService.SetUserRoles(user, roles.Roles);
            if (invalidRoles.Count > 0)
                return BadRequest(new MessageDto(string.Format(_translator[UserResourceConstants.InvalidRoles], string.Join(", ", invalidRoles))));

            var newRoles = _userService.ListUserRoles(user);

            return Ok(newRoles);
        }
        
        [HttpDelete]
        [Route("{id}/roles")]
        public async Task<ActionResult> RemoveUserRoles(Guid id, [FromBody] ManageUserRolesDto roles)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            var invalidRoles = await _userService.RemoveUserRoles(user, roles.Roles);
            if (invalidRoles.Count > 0)
                return BadRequest(new MessageDto(string.Format(_translator[UserResourceConstants.InvalidRoles], string.Join(", ", invalidRoles))));

            var newRoles = _userService.ListUserRoles(user);

            return Ok(newRoles);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateUser(Guid id, [FromBody] ManageUserDto updates)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            var updatedUser = await _userService.UpdateUser(user, updates);
            
            if(updatedUser == null)
                return BadRequest(new MessageDto(_translator[UserResourceConstants.UsernameAlreadyExists]));

            return Ok(updatedUser);
        }

        [HttpPost]
        [Route("{id}/password")]
        public async Task<ActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordDto passwordDto)
        {
            var user = await _userService.FindUserById(id);
            
            if (user == null)
                return NotFound(new MessageDto(_translator[UserResourceConstants.UserNotFound]));

            if (await _userService.ChangePassword(user, passwordDto))
                return Ok(new MessageDto(_translator[UserResourceConstants.PasswordChangedSuccessfully]));

            return BadRequest(new MessageDto(_translator[UserResourceConstants.CurrentPasswordIncorrect]));
        }
    }
}