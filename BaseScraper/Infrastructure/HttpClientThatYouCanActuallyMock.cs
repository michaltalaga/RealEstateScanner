namespace BaseScraper.Infrastructure;

public class HttpClientThatYouCanActuallyMock : IHttpClient
{
    private readonly IHttpClientFactory httpClientFactory;

    public HttpClientThatYouCanActuallyMock(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }
    public async Task<string> GetStringAsync(string url) => await httpClientFactory.CreateClient().GetStringAsync(url);
}