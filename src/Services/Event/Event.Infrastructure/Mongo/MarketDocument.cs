﻿using Event.Core.ValueObjects;

namespace Event.Infrastructure.Mongo;

public record MarketDocument(string Name, IEnumerable<SelectionDocument> SelectionDocuments, MarketLimitsDocument Limits);