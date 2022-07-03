using BaseScraper.Infrastructure;

namespace BaseScraper;

public class ListPageSourcePagingHttpDownloader : IListPageSource
{
    private readonly IHttpClient httpClient;

    public ListPageSourcePagingHttpDownloader(IHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async IAsyncEnumerable<ListPage> Get(string pageUrlFormatString, int maxPages = int.MaxValue)
    {
        var currentPage = 1;
        while (currentPage <= maxPages)
        {
            var listPageUrl = string.Format(pageUrlFormatString, currentPage);
            string rawHtml;
            try
            {
                rawHtml = await httpClient.GetStringAsync(listPageUrl);
            }
            catch (Exception)
            {
                yield break;
            }
            yield return new ListPage(Guid.NewGuid(), listPageUrl, rawHtml);
            currentPage++;

        }
    }
}