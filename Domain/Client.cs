namespace Domain
{

    public class Client
    {
        public required Guid ID { get; set; } = Guid.NewGuid();

        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

    }
} 