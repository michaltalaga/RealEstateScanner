namespace BaseScraper.Models;

public record Apartment
(
    string Id,
    string Source,
    string ExternalId,
    Address Address,
    PropertyData PropertyData
);