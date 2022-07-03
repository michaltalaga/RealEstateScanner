using BaseScraper;
using BaseScraper.Infrastructure;
using GratkaScraper;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((hostBuilderContext, serviceCollection) =>
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddTransient<IHttpClient, HttpClientThatYouCanActuallyMock>();
        //serviceCollection.AddSingleton<IListPageParser, IGratkaListPageParser>();
        //serviceCollection.AddTransient<IListPageFoundHandler, InListPageFoundHandler>();
        serviceCollection.AddTransient<IListPageFoundHandler, CosmosDbListPageFoundHandler>();
        serviceCollection.AddTransient<IListPageSource, ListPageSourcePagingHttpDownloader>();
        serviceCollection.AddTransient<IScraper, WebScraper>();
        serviceCollection.AddSingleton(x =>
        {
            var connectionString = hostBuilderContext.Configuration.GetConnectionString("CosmosDBConnection");
            return new CosmosClientBuilder(connectionString)
            .WithSerializerOptions(new CosmosSerializationOptions { PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase })
            .Build();
        });
        
    })
    .Build();
host.Run();