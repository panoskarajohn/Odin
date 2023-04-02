using Slip.Service.Domain;
using Slip.Service.Protos;

namespace Slip.Service.Chain.SlipChain;

public class SlipCheckAgainstGrpc : ISlipChain
{
    private readonly Event.EventClient _eventGrpcClient;
    private readonly ILogger<SlipCheckAgainstGrpc> _logger;
    
    public SlipCheckAgainstGrpc(Event.EventClient eventGrpcClient, ILogger<SlipCheckAgainstGrpc> logger)
    {
        _eventGrpcClient = eventGrpcClient;
        _logger = logger;
    }
    
    public async Task<bool> Handle(Domain.Slip slip, CancellationToken cancellationToken)
    {
        try
        {
            if(slip.Bets is null)
                throw new InvalidOperationException("Slip must have at least one bet");
            
            return await ValidateBets(slip.Bets);
        }
        catch (Exception e)
        {
            _logger.LogError("Error validating bets", e.Message);
            return false;
        }
    }
    
    private async Task<bool> ValidateBets(IEnumerable<Bet> bets)
    {
        foreach (var bet in bets)
        {
            foreach (var betSelection in bet.Selections)
            {
                _logger.LogInformation("Validating bet event {EventId}", betSelection.EventId);
                var request = new GetEventRequest() { Id = betSelection.EventId };
                var @event = await _eventGrpcClient.GetEventAsync(request);
                if (@event is null)
                {
                    _logger.LogInformation("Event {EventId} does not exist", betSelection.EventId);
                    return false;
                }
            }
        }
        return true;
    }
    
    
}