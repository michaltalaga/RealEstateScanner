using BaseScraper;
using Dasync.Collections;
using NSubstitute;

namespace BaseScraperTests;

public class WebScraperTests
{
    WebScraper webScraper;
    string pageUrlFormatString = "https://site.com/{0}";
    int maxPages = 5;
    ListPage[] pages = new ListPage[]
    {
        new ListPage(Guid.NewGuid(), "https://site.com/1", "dummy"),
        new ListPage(Guid.NewGuid(), "https://site.com/2", "dummy"),
    };
    IListPageSource listPageSource = Substitute.For<IListPageSource>();
    IListPageFoundHandler listPageFoundHandler = Substitute.For<IListPageFoundHandler>();

    public WebScraperTests()
    {
        webScraper = new WebScraper(listPageSource, listPageFoundHandler);
        listPageSource.Get(pageUrlFormatString, maxPages).Returns(pages.ToAsyncEnumerable());
    }

    [Fact]
    public async Task GetsPagesFromSource()
    {
        await webScraper.Scrape(pageUrlFormatString, maxPages);

        listPageSource.Received(1).Get(pageUrlFormatString, maxPages);
    }

    [Fact]
    public async Task PublishesEachPageFoundToHandler()
    {
        await webScraper.Scrape(pageUrlFormatString, maxPages);

        await listPageFoundHandler.Received(pages.Length).Found(Arg.Any<ListPage>());
    }
}