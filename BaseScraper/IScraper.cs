namespace BaseScraper;

public interface IScraper
{
    Task Scrape(string urlFormatString, int maxPages = int.MinValue);
}