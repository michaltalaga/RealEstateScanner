using Microsoft.Azure.Cosmos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseScraper;

internal class Class1
{
}

public interface IListPageFoundHandler
{
    Task Found(ListPage listPage);
}
public class InListPageFoundHandler : IListPageFoundHandler
{
    private readonly IListPageParser listPageParser;

    public InListPageFoundHandler(IListPageParser listPageParser)
    {
        this.listPageParser = listPageParser;
    }
    public async Task Found(ListPage listPage)
    {
        throw new NotImplementedException();
    }

}
public class CosmosDbListPageFoundHandler : IListPageFoundHandler
{
    private readonly CosmosClient cosmosClient;

    public CosmosDbListPageFoundHandler(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
    }
    public async Task Found(ListPage listPage)
    {
        var container = cosmosClient.GetContainer(Consts.CosmosDatabaseName, Consts.CosmosListPageContainerName);
        await container.CreateItemAsync(listPage);
    }
}
public interface IListPageParser
{
    public string[] GetDetailsPagesUrls(string rawHtml);
}
public class IGratkaListPageParser : IListPageParser
{
    public string[] GetDetailsPagesUrls(string rawHtml)
    {
        throw new NotImplementedException();
    }
}
public record ListPage(Guid Id, string Url, string RawHtml);