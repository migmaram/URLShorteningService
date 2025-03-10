using Microsoft.EntityFrameworkCore;
using URLShorteningService.Models;

namespace URLShorteningService.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options)
            : base(options)
        {

        }

        public DbSet<Url> Urls { get; set; }
        public DbSet<Visit> Visits { get; set; }
    }
}
