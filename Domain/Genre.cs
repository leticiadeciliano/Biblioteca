namespace Domain
{
    public class Genre
    {
        public int ID { get; set; }
        public required string Name_genre { get; set; }

        public DateTime Created_At { get; set; } = DateTime.Now;
        public DateTime Updated_At { get; set; } = DateTime.Now;

    }
}