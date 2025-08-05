using System;

namespace Domain
{

    public class Client
    {
        public required Guid Id { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

    }
} 