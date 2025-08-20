using System;

namespace Domain
{
    public class CatalogGenre
    {
        public required Guid ID { get; set; } = Guid.NewGuid();
        public required Guid CatalogID { get; set; }
        public required Guid GenreID { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

    }
}