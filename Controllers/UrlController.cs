using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using URLShorteningService.Data;
using URLShorteningService.Models;
using URLShorteningService.Tools;

namespace URLShorteningService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private ApiDbContext _context;
        public UrlController(ApiDbContext context)
        {
            _context = context;
        }
        [HttpPost("[action]")]
        public IActionResult Shorten([FromBody] string longUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newUniqeKey = Sequencer.GenerateKey(_context).Result;
            var today = DateTime.Now;

            var url = new Url
            {
                Key = newUniqeKey,
                LongUrl = longUrl,
                ShortUrl = $"https://shurlt.com/{newUniqeKey}",
                CreatedAt = today,
                UpdatedAt = today
            };

            _context.Urls.Add(url);
            _context.SaveChanges();

            return Created();
        }

    }
}
