namespace BaseScraper;

public class WebScraper : IScraper
{
    private readonly IListPageSource listPageSource;
    private readonly IListPageFoundHandler listPageFoundHandler;

    public WebScraper(IListPageSource listPageSource, IListPageFoundHandler listPageFoundHandler)
    {
        this.listPageSource = listPageSource;
        this.listPageFoundHandler = listPageFoundHandler;
    }

    public async Task Scrape(string urlFormatString, int maxPages = int.MinValue)
    {
        await foreach (var page in listPageSource.Get(urlFormatString, maxPages))
        {
            await listPageFoundHandler.Found(page);
        }
    }
}
