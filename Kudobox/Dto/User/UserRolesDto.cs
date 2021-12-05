using System.Collections.Generic;

namespace Kudobox.Dto.User
{
    public class UserRolesDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}