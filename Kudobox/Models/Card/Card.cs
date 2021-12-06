using System;
using Kudobox.Dto.Card;

namespace Kudobox.Models.Card
{
    public class Card
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }

        public Card(string name, string description, string icon, string color)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Icon = icon;
            Color = color;
            Active = true;
        }

        public CardDto ToDto()
        {
            return new CardDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Icon = Icon,
                Color = Color,
                Active = Active
            };
        }
    }
}