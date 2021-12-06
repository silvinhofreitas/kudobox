using System;

namespace Kudobox.Dto.Card
{
    public class CardDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public bool Active { get; set; }
    }
}