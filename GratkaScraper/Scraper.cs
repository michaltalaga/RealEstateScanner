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
    private readonly IListPageSource listPageSource;
    private readonly IListPageFoundHandler listPageFoundHandler;
    public Scraper(ILoggerFactory loggerFactory, IListPageSource listPageSource, IListPageFoundHandler listPageFoundHandler)
    {
        logger = loggerFactory.CreateLogger<Scraper>();
        this.listPageSource = listPageSource;
        this.listPageFoundHandler = listPageFoundHandler;
    }

    [Function("Scraper")]
    public async Task Run([TimerTrigger("0 */5 * * * *", RunOnStartup = runOnStartup)] TimerInfo myTimer)
    {
        var urlFormatString = "https://gratka.pl/nieruchomosci/mieszkania?cena-calkowita:min=1&sort=newest&page={0}";
        await foreach (var item in listPageSource.Get(urlFormatString))
        {
            await listPageFoundHandler.Found(item);
        }

    }

}
