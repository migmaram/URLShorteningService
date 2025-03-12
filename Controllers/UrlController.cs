using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
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
                VisitedAt = DateTime.Now });
            _context.SaveChanges();

            return Ok(url);
        }

        [HttpGet("Shorten/{key}/stats")]
        public IActionResult GetStats(string key)
        {

            var urlStats =
                from urls in _context.Urls
                where urls.Key == key
                join visits in _context.Visits on urls.Id equals visits.UrlId into urlVisits
                select new
                {
                    urls.Id,
                    url = urls.LongUrl,
                    shortCode = urls.Key,
                    urls.CreatedAt,
                    urls.UpdatedAt,
                    accessCount = urlVisits.ToList().Count()
                };

            if (!urlStats.Any())
                return NotFound();

            return Ok(urlStats);
        }

        [HttpPut("[action]/{key}")]
        public IActionResult Shorten([FromRoute] string key, [FromBody] Url updatedUrl)
        {
            var url = _context.Urls.FirstOrDefault(u => u.Key == key);

            if (url == null)
                return NotFound();

            url.LongUrl = updatedUrl.LongUrl;
            url.UpdatedAt = DateTime.Now;

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
