using BaseScraper;
using Moq;
using Moq.Contrib.HttpClient;

namespace BaseScraperTests;

public class ListPageSourcePagingHttpDownloaderTests
{
    Mock<HttpMessageHandler> handler;
    ListPageSourcePagingHttpDownloader downloader;

    public ListPageSourcePagingHttpDownloaderTests()
    {
        handler = new Mock<HttpMessageHandler>();
        var factory = handler.CreateClientFactory();
        downloader = new ListPageSourcePagingHttpDownloader(factory);
    }

    
    [Fact]
    public async Task PagesCorrectlyUsingUrlFormatString()
    {
        var maxPagesToDownload = 5;
        var sequence = new MockSequence();
        for (int i = 1; i <= maxPagesToDownload; i++)
        {
            handler.InSequence(sequence).SetupRequest($"https://site.com/{i}").ReturnsResponse("dummy");
        }
        
        await foreach (var _ in downloader.Get("https://site.com/{0}", maxPagesToDownload)) { }
    }
    [Fact]
    public async Task RespectsMaxPagesToDownload()
    {
        var maxPagesToDownload = 5;
        handler.SetupAnyRequest().ReturnsResponse("dummy");

        await foreach (var item in downloader.Get("https://site.com/{0}", maxPagesToDownload)) { }
        
        handler.VerifyAnyRequest(Times.Exactly(maxPagesToDownload));
    }

    [Fact]
    public async Task StopsWhenNoMorePages()
    {
        var sequence = new MockSequence();
        handler.InSequence(sequence).SetupRequest($"https://site.com/{1}").ReturnsResponse("dummy");
        handler.InSequence(sequence).SetupRequest($"https://site.com/{2}").ReturnsResponse("dummy");
        handler.InSequence(sequence).SetupRequest($"https://site.com/{3}").ReturnsResponse(System.Net.HttpStatusCode.NotFound);

        var results = 0;
        await foreach (var _ in downloader.Get("https://site.com/{0}", 5)) { results++; }

        handler.VerifyAnyRequest(Times.Exactly(3)); 
        Assert.Equal(2, results);
    }
}