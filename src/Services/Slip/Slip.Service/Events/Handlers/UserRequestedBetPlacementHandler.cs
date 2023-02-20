using Microsoft.EntityFrameworkCore;
using Shared.Cqrs.Events;
using Slip.Service.DAL;
using Slip.Service.Domain;
using Slip.Service.Events.Externals;
using Slip.Service.Mapper;
using Slip.Service.Protos;

namespace Slip.Service.Events.Handlers;

public class UserRequestedBetPlacementHandler : IEventHandler<UserRequestedBetPlacement>
{
    private readonly ILogger<UserRequestedBetPlacementHandler> _logger;
    private readonly Event.EventClient _eventGrpcClient;
    private readonly SlipContext _slipContext;
    public UserRequestedBetPlacementHandler(ILogger<UserRequestedBetPlacementHandler> logger, SlipContext slipContext, Event.EventClient eventGrpcClient)
    {
        _logger = logger;
        _slipContext = slipContext;
        _eventGrpcClient = eventGrpcClient;
    }
    
    public async Task HandleAsync(UserRequestedBetPlacement @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("User {UserId} requested bet placement for Slip {SlipId}", @event.UserId, @event.Slip.Id);

        var slip = @event.Slip.ToDomain();

        var slipExists = await 
            _slipContext.Slips
            .AnyAsync(s => s.Id == slip.Id, cancellationToken);
        
        if(slipExists)
        {
            _logger.LogInformation("Slip {SlipId} already exists", slip.Id);
            return;
        }
        
        if(slip.Bets is null)
            throw new InvalidOperationException("Slip must have at least one bet");

        if (!await ValidateBets(slip.Bets)) return;

        await _slipContext.Slips.AddAsync(slip, cancellationToken);
    }
    
    private async Task<bool> ValidateBets(IEnumerable<Bet> bets)
    {
        try
        {
            foreach (var bet in bets)
            {
                foreach (var betSelection in bet.Selections)
                {
                    _logger.LogInformation("Validating bet event {EventId}", betSelection.EventId);
                    var request = new GetEventRequest() { Id = betSelection.EventId };
                    var @event = await _eventGrpcClient.GetEventAsync(request);
                    if (@event is null)
                        return false;
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error validating bets", e.Message);
            return false;
        }
        
        return true;
    }
}