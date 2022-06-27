using BaseScraper;
using Dasync.Collections;
using Moq;
namespace BaseScraperTests;

public class WebScraperTests
{
    WebScraper webScraper;
    string pageUrlFormatString = "https://site.com/{0}";
    int maxPages = 5;
    ListPage[] pages = new ListPage[]
    {
        new ListPage("https://site.com/1", "dummy"),
        new ListPage("https://site.com/1", "dummy"),
    };
    Mock<IListPageSource> listPageSourceMock = new();
    Mock<IListPageFoundHandler> listPageFoundHandlerMock = new();

    public WebScraperTests()
    {
        webScraper = new WebScraper(listPageSourceMock.Object, listPageFoundHandlerMock.Object);
    }

    [Fact]
    public async Task GetsPagesFromSource()
    {
        listPageSourceMock.Setup(x => x.Get(pageUrlFormatString, maxPages)).Returns(pages.ToAsyncEnumerable());

        await webScraper.Scrape(pageUrlFormatString, maxPages);

        listPageSourceMock.Verify(x => x.Get(pageUrlFormatString, maxPages), Times.Once);
    }

    [Fact]
    public async Task PublishesEachPageFoundToHandler()
    {
        listPageSourceMock.Setup(x => x.Get(pageUrlFormatString, maxPages)).Returns(pages.ToAsyncEnumerable());

        await webScraper.Scrape(pageUrlFormatString, maxPages);

        listPageFoundHandlerMock.Verify(x => x.Found(It.IsAny<ListPage>()), Times.Exactly(pages.Length));
    }
}
