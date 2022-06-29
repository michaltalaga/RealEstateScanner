using System;
using BaseScraper;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace GratkaScraper;

public class Scraper
{
#if DEBUG
    const bool runOnStartup = true;
#else
    const bool runOnStartup = false;
#endif

    private readonly ILogger logger;
    private readonly IScraper scraper;

    public Scraper(ILoggerFactory loggerFactory, IScraper scraper)
    {
        logger = loggerFactory.CreateLogger<Scraper>();
        this.scraper = scraper;
    }

    [Function("Scraper")]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = runOnStartup)] TimerInfo myTimer)
    {
        var urlFormatString = "https://gratka.pl/nieruchomosci/mieszkania?cena-calkowita:min=1&sort=newest&page={0}";
        //await scraper.Scrape(urlFormatString, 2);
    }

    [Function("ListPageFound")]
    public async Task Run1([CosmosDBTrigger(
            databaseName: Consts.CosmosDatabaseName,
            collectionName: Consts.CosmosListPageContainerName,
            ConnectionStringSetting = Consts.ConnectionStringName,
            LeaseCollectionName = "leases", CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<ListPage> input)
    {
        return;
    }
}