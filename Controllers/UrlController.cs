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
        public async Task<IActionResult> Shorten([FromBody] Url originalUrl)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var newUniqeKey = await _sequencer.GenerateKeyAsync();
            var today = DateTime.Now;

            var url = new Url
            {
                Key = newUniqeKey,
                LongUrl = originalUrl.LongUrl,
                ShortUrl = $"https://shurlt.com/{newUniqeKey}",
                CreatedAt = today,
                UpdatedAt = today
            };

            await _unitOfWork.Urls.AddAsync(url);
            await _unitOfWork.SaveAsync();

            return StatusCode(StatusCodes.Status201Created, url);
        }

        [HttpGet("[action]/{key}")]
        public async Task<IActionResult> Shorten([FromRoute] string key)
        {
            var url = await _unitOfWork.Urls.GetByKeyAsync(key);

            if (url == null)
                return NotFound();

            await _unitOfWork.Visits.AddAsync(
                new Visit
                {
                    UrlId = url.Id,
                    VisitedAt = DateTime.Now
                }
            );

            await _unitOfWork.SaveAsync();

            return Ok(url);
        }

        [HttpGet("Shorten/{key}/stats")]
        public async Task<IActionResult> GetStats(string key)
        {
            var url = await _unitOfWork.Urls.GetByKeyAsync(key);

            if (url == null)
                return NotFound();

            var urlStats = new
            {
                url.Id,
                url.LongUrl,
                url.Key,
                url.CreatedAt,
                url.UpdatedAt,
                accessCount = await _unitOfWork.Visits.FilteredCountAsync(visit => 
                    visit.UrlId == url.Id)
            };

            return Ok(urlStats);
        }

        [HttpPut("[action]/{key}")]
        public async Task<IActionResult> Shorten([FromRoute] string key, [FromBody] Url url)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var existingUrl =await _unitOfWork.Urls.GetByKeyAsync(key);

            if (existingUrl == null)
                return NotFound();

            existingUrl.LongUrl = url.LongUrl;
            existingUrl.UpdatedAt = DateTime.Now;

            _unitOfWork.Urls.Update(existingUrl);
            await _unitOfWork.SaveAsync();

            return Ok(existingUrl);
        }


        [HttpDelete("Shorten/{key}")]
        public async Task<IActionResult> Delete([FromRoute] string key)
        {
            var deleted = await _unitOfWork.Urls.DeleteByKeyAsync(key);

            if (!deleted)
                return NotFound();

            await _unitOfWork.SaveAsync();
            return NoContent();
        }
    }
}
