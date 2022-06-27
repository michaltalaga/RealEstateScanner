using BaseScraper;
using GratkaScraper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(serviceCollection =>
    {
        serviceCollection.AddHttpClient();
        serviceCollection.AddTransient<IListPageFoundHandler, InMemoryGratkaListPageFoundHandler>();
        serviceCollection.AddTransient<IListPageSource, ListPageSourcePagingHttpDownloader>();
    })
    .Build();

host.Run();