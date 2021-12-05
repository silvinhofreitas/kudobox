using System.ComponentModel.DataAnnotations;

namespace Kudobox.Dto.User
{
    public class ChangePasswordDto
    {
        [Required]
        public string OldPassword { get; set; }
        
        [Required]
        public string NewPassword { get; set; }
    }
}