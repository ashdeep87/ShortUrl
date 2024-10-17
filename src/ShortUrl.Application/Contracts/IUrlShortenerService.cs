namespace ShortUrl.Application.Contracts
{
    public interface IUrlShortenerService
    {
        string ResolveUrl(string shortId);
        string ShortenUrl(string originalUrl);
    }
}