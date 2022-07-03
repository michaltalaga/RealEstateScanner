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

public interface IItemFoundSink
{
    Task Found(object item);
}
public class InListPageFoundHandler : IItemFoundSink
{
    private readonly IListPageParser listPageParser;

    public InListPageFoundHandler(IListPageParser listPageParser)
    {
        this.listPageParser = listPageParser;
    }
    public async Task Found(object item)
    {
        throw new NotImplementedException();
    }

}
public class CosmosDbItemFoundSink : IItemFoundSink
{
    private readonly CosmosClient cosmosClient;

    public CosmosDbItemFoundSink(CosmosClient cosmosClient)
    {
        this.cosmosClient = cosmosClient;
    }
    public async Task Found(object item)
    {
        var container = cosmosClient.GetContainer(Consts.CosmosDatabaseName, Consts.CosmosListPageContainerName);
        await container.CreateItemAsync(item);
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