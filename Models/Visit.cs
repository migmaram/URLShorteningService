using System.ComponentModel.DataAnnotations;

namespace URLShorteningService.Models
{
    public class Visit
    {
        public int Id { get; set; }
        public Url? Url{ get; set; }
        public int UrlId { get; set; }
        public DateTime VisitedAt { get; set; }
    }
}
