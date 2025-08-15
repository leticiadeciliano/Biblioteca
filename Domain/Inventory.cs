using System;

namespace Domain
{
    public class Inventory
    {
        public required Guid ID { get; set; } = Guid.NewGuid();
        public required int Condition { get; set; }
        public required bool Is_foreign { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // comando usado para se referir a Foreign Key
        public required Guid CatalogID { get; set; }
    }
}