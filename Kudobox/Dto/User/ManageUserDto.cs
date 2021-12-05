using System;
using System.ComponentModel.DataAnnotations;

namespace Kudobox.Dto.User
{
    public class ManageUserDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Username { get; set; }
        
        [MaxLength(30)]
        public string DisplayName { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public string Avatar { get; set; }
        
        public DateTime Birthday { get; set; }
    }
}