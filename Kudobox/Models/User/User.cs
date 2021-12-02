using System;
using Kudobox.Helpers.Enums;

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
        public UserStatusEnum Status { get; set; }
        public string Roles { get; set; }
    }
}