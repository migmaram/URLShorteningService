using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml;

namespace URLShorteningService.Models
{
    [Index(nameof(Key), IsUnique = true)]
    public class Url
    {
        public int Id { get; set; }
        public string? Key { get; set; }
        [Required]
        [JsonPropertyName("Url")]
        public string? LongUrl { get; set; }
        public string? ShortUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
