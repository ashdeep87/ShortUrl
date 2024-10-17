using Microsoft.Extensions.Configuration;
using ShortUrl.Application.Contracts;
using System.Collections.Concurrent;

namespace ShortUrl.Application.Services
{
    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly ConcurrentDictionary<string, string> _urlMappings;
        private readonly string _domainUrl;

        public UrlShortenerService(ConcurrentDictionary<string, string> urlMappings, IConfiguration configuration)
        {
            _urlMappings = urlMappings;
            _domainUrl = $"{configuration["DOMAIN_URL"]}";
        }

        public string ShortenUrl(string originalUrl)
        {
            var shortId = GenerateShortId();
            _urlMappings[shortId] = originalUrl;
            return $"{_domainUrl}/{shortId}";
        }

        public string ResolveUrl(string shortId)
        {
            _urlMappings.TryGetValue(shortId, out var originalUrl);
            return originalUrl;
        }

        private string GenerateShortId()
        {
            var shortId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return shortId.Substring(0, 8).Replace("/", "_").Replace("+", "-");
        }
    }
}