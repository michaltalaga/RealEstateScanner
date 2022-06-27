using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BaseScraper;

internal class Class1
{
}

public interface IListPageFoundHandler
{
    Task Found(ListPage listPage);
}
public class InMemoryGratkaListPageFoundHandler : IListPageFoundHandler
{
    public async Task Found(ListPage listPage)
    {
        var detailsPagesUrls = GetDetailsPagesUrls(listPage.RawHtml);
    }

    private string[] GetDetailsPagesUrls(string rawHtml)
    {
        return Array.Empty<string>();
    }
}
public record ListPage(string Url, string RawHtml);