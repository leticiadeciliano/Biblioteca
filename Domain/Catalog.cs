using System;


namespace Domain
{
    public class Catalog
    {
        public required Guid ID { get; set; } = Guid.NewGuid();
        public required string Title { get; set; }
        public required string Author { get; set; }
        public int Number_pages { get; set; }
        public required int Year { get; set; }
        public required string Description { get; set; }
        public required string Publisher_ID { get; set; }
        public required string Language_ID { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;
    }
}