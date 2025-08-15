using System;

namespace Domain
{
    public class Publisher
    {
        public required Guid ID { get; set; } = Guid.NewGuid();
        public required string Name_Publisher { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}