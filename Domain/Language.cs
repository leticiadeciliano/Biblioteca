namespace Domain
{
    public class Language
    {
        public int ID { get; set; }
        public required string Name { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;
    }
}