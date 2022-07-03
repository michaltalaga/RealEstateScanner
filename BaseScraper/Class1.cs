using BaseScraper.Infrastructure;
using HtmlAgilityPack;
using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;

namespace BaseScraper;

internal class Class1
{
}

public interface IItemFoundSink
{
    Task Found(object item);
}
//public class InListPageFoundHandler : IItemFoundSink
//{
//    private readonly IListPageParser listPageParser;

//    public InListPageFoundHandler(IListPageParser listPageParser)
//    {
//        this.listPageParser = listPageParser;
//    }
//    public async Task Found(object item)
//    {
//        throw new NotImplementedException();
//    }

//}
public class CosmosDbItemFoundSink : IItemFoundSink
{
    private readonly CosmosClient cosmosClient;

    public CosmosDbItemFoundSink(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
    }
    public async Task Found(object item)
    {
        var container = cosmosClient.GetContainer(Consts.CosmosDatabaseName, item.GetType().Name);
        await container.CreateItemAsync(item);
    }
}
public interface IListPageParser
{
    public string[] GetDetailsPagesUrls(string rawHtml);
}
public class GratkaListPageParser : IListPageParser
{
    public string[] GetDetailsPagesUrls(string rawHtml)
    {
        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(rawHtml);

        var detailPagesHyperLinkNodes = htmlDoc.DocumentNode.SelectNodes("//*[@class='listing']//a[@class='teaserLink']");
        return detailPagesHyperLinkNodes.Select(a => a.Attributes["href"].Value).ToArray();
        //var getText = (string selector) => HttpUtility.HtmlDecode(tableNode.SelectSingleNode(selector).InnerText.Trim());
        //var getTextFromNextCellByLabel = (string label) => HttpUtility.HtmlDecode(tableNode.SelectSingleNode($"//*[contains(text(), '{label}')]/parent::td/following-sibling::td").InnerText.Trim());
        //var getBoolFromNextCellByLabel = (string label) => getTextFromNextCellByLabel(label) == "No" ? false : true;
    }
}
public interface IListPageFoundHandler
{
    Task Handle(string rawHtml);
}
public class ListPageFoundHandler : IListPageFoundHandler
{
    private readonly IListPageParser listPageParser;
    private readonly IHttpClient httpClient;
    private readonly IItemFoundSink itemFoundSink;

    public ListPageFoundHandler(IListPageParser listPageParser, IHttpClient httpClient, IItemFoundSink itemFoundSink)
    {
        this.listPageParser = listPageParser;
        this.httpClient = httpClient;
        this.itemFoundSink = itemFoundSink;
    }
    public async Task Handle(string rawHtml)
    {
        foreach (var url in listPageParser.GetDetailsPagesUrls(rawHtml))
        {
            var detailsPageRawHtml = await httpClient.GetStringAsync(url);
            await itemFoundSink.Found(new DetailsPage(Guid.NewGuid(), url, detailsPageRawHtml));
        }
    }
}
public record ListPage(Guid Id, string Url, string RawHtml);
public record DetailsPage(Guid Id, string Url, string RawHtml);