using BaseScraper;
using NSubstitute;

namespace BaseScraperTests;

public class ListPageSourcePagingHttpDownloaderTests
{
    IHttpClient httpClient = Substitute.For<IHttpClient>();
    ListPageSourcePagingHttpDownloader downloader;


    public ListPageSourcePagingHttpDownloaderTests()
    {
        httpClient.GetStringAsync(Arg.Any<string>()).Returns(Task.FromException<string>(new Exception()));
        downloader = new ListPageSourcePagingHttpDownloader(httpClient);
    }

    [Fact]
    public async Task PagesCorrectlyUsingUrlFormatString()
    {
        const int maxPagesToDownload = 5;
        for (int i = 1; i <= maxPagesToDownload; i++)
        {
            httpClient.GetStringAsync($"https://site.com/{i}").Returns("page" + i);
        }
        await foreach (var _ in downloader.Get("https://site.com/{0}", maxPagesToDownload)) { }
        await httpClient.Received(maxPagesToDownload).GetStringAsync(Arg.Any<string>());
    }

    [Fact]
    public async Task StopsWhenNoMorePages()
    {
        
        httpClient.GetStringAsync($"https://site.com/{1}").Returns("dummy");
        httpClient.GetStringAsync($"https://site.com/{2}").Returns("dummy");
        httpClient.GetStringAsync($"https://site.com/{3}").Returns(Task.FromException<string>(new Exception()));

        var results = 0;
        await foreach (var _ in downloader.Get("https://site.com/{0}", 5)) { results++; }

        await httpClient.Received(3).GetStringAsync(Arg.Any<string>());
        Assert.Equal(2, results);
    }
}