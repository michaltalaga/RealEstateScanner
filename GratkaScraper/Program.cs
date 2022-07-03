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
        serviceCollection.AddTransient<IItemFoundSink, CosmosDbItemFoundSink>();
        serviceCollection.AddTransient<IListPageSource, ListPageSourcePagingHttpDownloader>();
        serviceCollection.AddTransient<IScraper, WebScraper>();
        serviceCollection.AddTransient<IListPageFoundHandler, ListPageFoundHandler>();
        serviceCollection.AddTransient<IListPageParser, GratkaListPageParser>();
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