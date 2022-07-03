namespace BaseScraper.Infrastructure;

public interface IHttpClient
{
    Task<string> GetStringAsync(string url);
}