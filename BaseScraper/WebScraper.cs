namespace BaseScraper;

public class WebScraper : IScraper
{
    private readonly IListPageSource listPageSource;
    private readonly IItemFoundSink itemFoundSink;

    public WebScraper(IListPageSource listPageSource, IItemFoundSink itemFoundSink)
    {
        this.listPageSource = listPageSource;
        this.itemFoundSink = itemFoundSink;
    }

    public async Task Scrape(string urlFormatString, int maxPages = int.MinValue)
    {
        await foreach (var page in listPageSource.Get(urlFormatString, maxPages))
        {
            await itemFoundSink.Found(page);
        }
    }
}