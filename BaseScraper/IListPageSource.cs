namespace BaseScraper;

public interface IListPageSource
{
    IAsyncEnumerable<ListPage> Get(string pageUrlFormatString, int maxPagesToDownload = int.MaxValue);
}
