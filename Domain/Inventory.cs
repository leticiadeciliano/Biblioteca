using System;

namespace Domain
{
    public class Inventory
    {
        public int ID { get; set; }
        public required int Condition { get; set; }
        public required bool Is_available { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

        // comando usado para se referir a Foreign Key
        public required Guid CatalogID { get; set; }
    }
}