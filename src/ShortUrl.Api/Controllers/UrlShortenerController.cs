using Microsoft.AspNetCore.Mvc;
using ShortUrl.Application.Contracts;
using ShortUrl.Core.Models;

namespace ShortUrl.Api.Models
{
    [ApiController]
    [Route("api")]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortenerService;

        public UrlShortenerController(IUrlShortenerService urlShortenerService)
        {
            _urlShortenerService = urlShortenerService;
        }

        [HttpPost("shorten")]
        public IActionResult ShortenUrl([FromBody] UrlRequest request)
        {
            if (!Uri.IsWellFormedUriString(request.OriginalUrl, UriKind.Absolute))
            {
                return BadRequest("Invalid URL format.");
            }

            var shortUrl = _urlShortenerService.ShortenUrl(request.OriginalUrl);
            return Ok(new { Url = shortUrl });
        }

        [HttpGet("{shortId}")]
        public IActionResult ResolveUrl(string shortId)
        {
            var originalUrl = _urlShortenerService.ResolveUrl(shortId);
            if (originalUrl == null)
            {
                return NotFound();
            }
            return Ok(new { Url = originalUrl });
        }
    }
}