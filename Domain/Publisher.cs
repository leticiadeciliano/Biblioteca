using System;

namespace Domain
{
    public class Publisher
    {
        public int ID { get; set; }
        public required string Name_Publisher { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}