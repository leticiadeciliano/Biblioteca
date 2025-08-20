using System;

namespace Domain
{
    public class Language
    {
        public int ID { get; set; }
        public required string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Foreign Key
        public required Guid LanguageID { get; set; }
    }
}