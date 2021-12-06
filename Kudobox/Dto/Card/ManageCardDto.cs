using System.ComponentModel.DataAnnotations;

namespace Kudobox.Dto.Card
{
    public class ManageCardDto
    {
        [Required]
        public string Description { get; set; }
        
        [Required]
        public string Icon { get; set; }
        
        [Required]
        public string Color { get; set; }
    }
}