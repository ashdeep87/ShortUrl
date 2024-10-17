using System.Collections.Concurrent;
using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using ShortUrl.Application.Services;

namespace ShortUrl.Test
{
    public class UrlShortenerServiceTests
    {
        private readonly UrlShortenerService _urlShortenerService;
        private readonly ConcurrentDictionary<string, string> _urlMappings;
        private readonly string _baseUrl = "http://localhost:5000";

        public UrlShortenerServiceTests()
        {
            _urlMappings = new ConcurrentDictionary<string, string>();

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(c => c["DOMAIN_URL"]).Returns(_baseUrl);

            _urlShortenerService = new UrlShortenerService(_urlMappings, configMock.Object);
        }

        [Fact]
        public void ShortenUrl_ShouldReturnValidShortUrl()
        {
            // Arrange
            var originalUrl = "https://example.com";

            // Act
            var shortUrl = _urlShortenerService.ShortenUrl(originalUrl);
            var shortId = shortUrl.Split('/').Last();
            // Assert
            Assert.StartsWith(_baseUrl, shortUrl);
            Assert.True(shortId.Length == 8);
            Assert.NotNull(shortUrl);
        }

        [Fact]
        public void ShortenUrl_ShouldGenerateUniqueShortIdForSameUrl()
        {
            // Arrange
            var originalUrl = "https://example.com";

            // Act
            var shortUrl1 = _urlShortenerService.ShortenUrl(originalUrl);
            var shortUrl2 = _urlShortenerService.ShortenUrl(originalUrl);

            // Assert
            Assert.NotEqual(shortUrl1, shortUrl2); // Each request should generate a unique short URL
        }

        [Fact]
        public void ResolveUrl_ShouldReturnOriginalUrlForExistingShortId()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var shortUrl = _urlShortenerService.ShortenUrl(originalUrl);
            var shortId = shortUrl.Split('/').Last();

            // Act
            var resolvedUrl = _urlShortenerService.ResolveUrl(shortId);

            // Assert
            Assert.Equal(originalUrl, resolvedUrl);
        }

        [Fact]
        public void ResolveUrl_ShouldReturnNullForNonExistentShortId()
        {
            // Act
            var resolvedUrl = _urlShortenerService.ResolveUrl("nonexistent");

            // Assert
            Assert.Null(resolvedUrl); // Should return null for a non-existent short ID
        }
    }
}