using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
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
        public IActionResult Shorten([FromBody] Url originalUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newUniqeKey = Sequencer.GenerateKey(_context).Result;
            var today = DateTime.Now;

            var url = new Url
            {
                Key = newUniqeKey,
                LongUrl = originalUrl.LongUrl,
                ShortUrl = $"https://shurlt.com/{newUniqeKey}",
                CreatedAt = today,
                UpdatedAt = today
            };

            _context.Urls.Add(url);
            _context.SaveChanges();

            return StatusCode(StatusCodes.Status201Created, url);
        }

        [HttpGet("[action]/{key}")]
        public IActionResult Shorten([FromRoute] string key)
        {
            var url = _context.Urls.FirstOrDefault(u => u.Key == key);
            
            if (url == null)
                return NotFound();

            _context.Visits.Add(new Visit { UrlId = url.Id, 
                VisitedAt = DateTime.Today });
            _context.SaveChanges();

            return Ok(url);
        }

        [HttpPut("[action]/{key}")]
        public IActionResult Shorten([FromRoute] string key, [FromBody] Url updatedUrl)
        {
            var url = _context.Urls.FirstOrDefault(u => u.Key == key);

            if (url == null)
                return NotFound();

            url.LongUrl = updatedUrl.LongUrl;

            _context.Urls.Update(url);
            _context.SaveChanges();

            return Ok(url);
        }


        [HttpDelete("Shorten/{key}")]
        public IActionResult Delete([FromRoute] string key)
        {
            var url = _context.Urls.FirstOrDefault(u => u.Key == key);

            if (url == null)
                return NotFound();

            _context.Urls.Remove(url);
            _context.SaveChanges();

            return Ok(url);
        }
    }
}
