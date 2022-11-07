using Event.Application.SportMatch.Features.GetMatch;
using Event.Core.ValueObjects;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Shared.Cqrs;

namespace Event.Grpc.Services;

internal sealed class EventGrpcService : Event.EventBase
{
    private readonly ILogger<EventGrpcService> _logger;
    private readonly IDispatcher _dispatcher;

    public EventGrpcService(ILogger<EventGrpcService> logger, IDispatcher dispatcher)
    {
        _logger = logger;
        _dispatcher = dispatcher;
    }

    public override async Task<EventResponse> GetEvent(GetEventRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Fetching event with id {EventId}", request.Id);
        var query = new GetMatchQuery(request.Id);
        var result = await _dispatcher.QueryAsync(query);

        if (result is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Event not found"));
        }

        return new EventResponse()
        {
            Id = result.Id,
            Name = result.MatchName,
            Category = result.Category,
            Starting = Timestamp.FromDateTime(result.StartingTime),
            Markets = { result.MarketDtos.Select(m => new Market()
            {
                Name = m.Name,
                Selections = { m.SelectionDtos.Select(s => new Selection()
                {
                    Name = s.Name,
                    Price = s.Price
                }) 
                }
            }) 
            }
        };
    }
}