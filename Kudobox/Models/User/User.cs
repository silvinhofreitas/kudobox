using System;
using Kudobox.Dto.User;
using Kudobox.Helpers.Constants;
using Kudobox.Helpers.Enums;
using Kudobox.Helpers.Extensions;

namespace Kudobox.Models.User
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public DateTime Birthday { get; set; }
        public UserStatusEnum Status { get; set; }
        public string Roles { get; set; }

        // TODO Remove this generic constructor
        public User()
        {
        }
        
        public User(string username, string decryptedPassword, string displayName, string name, string surname, string email,
            string avatar, DateTime birthday)
        {
            Id = Guid.NewGuid();
            Username = username;
            Password = decryptedPassword.Encrypt();
            DisplayName = displayName;
            Name = name;
            Surname = surname;
            Email = email;
            Avatar = avatar;
            Birthday = birthday;
            Status = UserStatusEnum.FirstAccess;
            Roles = RoleConstants.USER;
        }

        public UserDto ToDto()
        {
            return new UserDto
            {
                Id = Id.ToString(),
                Username = Username,
                Name = Name,
                Surname = Surname,
                Avatar = Avatar,
                Birthday = Birthday.ToShortDateString(),
                DisplayName = DisplayName,
                Email = Email,
                Status = Status.ToString()
            };
        }
    }
}