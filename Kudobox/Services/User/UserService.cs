using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kudobox.Contexts;
using Kudobox.Dto.Shared;
using Kudobox.Dto.User;
using Kudobox.Helpers.Constants;
using Kudobox.Helpers.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Kudobox.Services.User
{
    public class UserService
    {
        private readonly UserContext _userContext;

        public UserService(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<Models.User.User> Login(UserLoginDto input)
        {
            return await _userContext.Users
                .FirstOrDefaultAsync(u => u.Username.Equals(input.Username) && u.Password.Equals(input.Password.Encrypt()));
        }
        
        public async Task<(UserDto, string tempPassword)> CreateNewUser(ManageUserDto input)
        {
            if (await _userContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(input.Username)) != null)
                return (null, string.Empty);
            
            var randomPassword = PasswordGenerator.Generate(10, 3);
            var user = await _userContext.AddAsync(new Models.User.User(input.Username, randomPassword, input.DisplayName, input.Name,
                input.Surname, input.Email, input.Avatar, input.Birthday));

            await _userContext.SaveChangesAsync();
            return (user.Entity.ToDto(), randomPassword);
        }

        public async Task<Models.User.User> FindUserById(Guid id)
        {
            return await _userContext.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
        }
        
        public async Task<PagedResultDto> GetAllUsers(int page, int pageSize)
        {
            var userList = await _userContext.Users.GetPagedAsync(page, pageSize);

            return new PagedResultDto
            {
                Page = userList.CurrentPage,
                PageCount = userList.PageCount,
                PageSize = userList.PageSize,
                HasNext = page < userList.PageCount,
                HasPrevious = page > 1,
                TotalItemsCount = userList.RowCount,
                Items = userList.Results.Select(ul => ul.ToDto()).ToList()
            };
        }

        public async Task DeleteUser(Models.User.User user)
        {
            _userContext.Users.Remove(user);
            await _userContext.SaveChangesAsync();
        }

        public UserRolesDto ListUserRoles(Models.User.User user)
        {
            return new UserRolesDto
            {
                Id = user.Id.ToString(),
                Username = user.Username,
                Roles = user.Roles.Split(';').ToList().OrderBy(ur => ur)
            };
        }

        public async Task<List<string>> SetUserRoles(Models.User.User user, IEnumerable<string> roles)
        {
            var invalidRoles = roles.Where(role => !RoleConstants.ValidRole(role)).ToList();
            if (invalidRoles.Count > 0)
                return invalidRoles;

            var actualRoles = user.Roles.Split(';').ToList();
            user.Roles += ";" + string.Join(";", roles.Where(r => !actualRoles.Contains(r)).ToArray());
            if (user.Roles.Substring(user.Roles.Length - 1, 1) == ";")
                user.Roles = user.Roles.Remove(user.Roles.Length - 1);

            await _userContext.SaveChangesAsync();
            
            return invalidRoles;
        }

        public async Task<List<string>> RemoveUserRoles(Models.User.User user, IEnumerable<string> roles)
        {
            var invalidRoles = roles.Where(role => !RoleConstants.ValidRole(role)).ToList();
            if (invalidRoles.Count > 0)
                return invalidRoles;
            
            var actualRoles = user.Roles.Split(';').ToList();
            var newRoles = actualRoles.Where(ar => !roles.Contains(ar)).ToList(); 
            user.Roles = string.Join(";", newRoles);

            await _userContext.SaveChangesAsync();
            
            return invalidRoles;
        }

        public async Task<UserDto> UpdateUser(Models.User.User user, ManageUserDto updates)
        {
            if (user.Username != updates.Username &&
                await _userContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(updates.Username)) != null)
                return null;

            user.Username = updates.Username;
            user.DisplayName = updates.DisplayName;
            user.Name = updates.Name;
            user.Surname = updates.Surname;
            user.Email = updates.Email;
            user.Avatar = updates.Avatar;
            user.Birthday = updates.Birthday;

            await _userContext.SaveChangesAsync();

            return user.ToDto();
        }

        public async Task<bool> ChangePassword(Models.User.User user, ChangePasswordDto passwordDto)
        {
            if (passwordDto.OldPassword.Encrypt() != user.Password)
                return false;

            user.Password = passwordDto.NewPassword.Encrypt();
            await _userContext.SaveChangesAsync();

            return true;
        }
    }
}