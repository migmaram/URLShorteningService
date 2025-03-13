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
        private readonly IUnitOfWork _unitOfWork;
        private readonly Sequencer _sequencer;

        public UrlController(IUnitOfWork unitOfWork, Sequencer sequencer)
        {
            _unitOfWork = unitOfWork;
            _sequencer = sequencer;
        }

        [HttpPost("[action]")]
        public IActionResult Shorten([FromBody] Url originalUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newUniqeKey = _sequencer.GenerateKey();
            var today = DateTime.Now;

            var url = new Url
            {
                Key = newUniqeKey,
                LongUrl = originalUrl.LongUrl,
                ShortUrl = $"https://shurlt.com/{newUniqeKey}",
                CreatedAt = today,
                UpdatedAt = today
            };

            _unitOfWork.Urls.Add(url);
            _unitOfWork.Save();

            return StatusCode(StatusCodes.Status201Created, url);
        }

        [HttpGet("[action]/{key}")]
        public IActionResult Shorten([FromRoute] string key)
        {
            var url = _unitOfWork.Urls.GetByKey(key);

            if (url == null)
                return NotFound();

            _unitOfWork.Visits.Add(
                new Visit
                {
                    UrlId = url.Id,
                    VisitedAt = DateTime.Now
                });

            _unitOfWork.Save();

            return Ok(url);
        }

        [HttpGet("Shorten/{key}/stats")]
        public IActionResult GetStats(string key)
        {
            var url = _unitOfWork.Urls.GetByKey(key);

            if (url == null)
                return NotFound();

            var urlStats = new
            {
                url.Id,
                url.LongUrl,
                url.Key,
                url.CreatedAt,
                url.UpdatedAt,
                accessCount = _unitOfWork.Visits.FilteredCount(visit => 
                    visit.UrlId == url.Id)
            };

            return Ok(urlStats);
        }

        [HttpPut("[action]/{key}")]
        public IActionResult Shorten([FromRoute] string key, [FromBody] Url url)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var existingUrl = _unitOfWork.Urls.GetByKey(key);

            if (existingUrl == null)
                return NotFound();

            existingUrl.LongUrl = url.LongUrl;
            existingUrl.UpdatedAt = DateTime.Now;

            _unitOfWork.Urls.Update(existingUrl);
            _unitOfWork.Save();

            return Ok(existingUrl);
        }


        [HttpDelete("Shorten/{key}")]
        public IActionResult Delete([FromRoute] string key)
        {
            var deleted = _unitOfWork.Urls.DeleteByKey(key);

            if (!deleted)
                return NotFound();

            _unitOfWork.Save();
            return NoContent();
        }
    }
}
