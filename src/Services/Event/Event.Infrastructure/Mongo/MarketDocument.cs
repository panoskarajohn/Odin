namespace Event.Infrastructure.Mongo;

public record MarketDocument(string Name, IEnumerable<SelectionDocument> SelectionDocuments);