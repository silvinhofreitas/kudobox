using System.ComponentModel.DataAnnotations;

namespace Kudobox.Dto.User
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }
    
        [Required]
        public string Password { get; set; }
    }
}