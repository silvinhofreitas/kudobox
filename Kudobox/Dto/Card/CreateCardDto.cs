using System.ComponentModel.DataAnnotations;

namespace Kudobox.Dto.Card
{
    public class CreateCardDto : ManageCardDto
    {
        [Required]
        public string Name { get; set; }
    }
}