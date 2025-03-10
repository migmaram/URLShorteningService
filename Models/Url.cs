namespace URLShorteningService.Models
{
    public class Url
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public required string LongUrl { get; set; }
        public required string ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Deleted { get; set; }

    }
}
