using System;

namespace Domain
{
    public class Language
    {
        public required Guid ID { get; set; } = Guid.NewGuid();
        public required string Name_language { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Foreign Key
        public required Guid LanguageID { get; set; }
    }
}
