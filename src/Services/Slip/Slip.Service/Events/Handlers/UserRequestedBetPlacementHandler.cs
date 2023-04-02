using Shared.Cqrs.Events;
using Slip.Service.Chain.SlipChain;
using Slip.Service.DAL;
using Slip.Service.Events.Externals;
using Slip.Service.Mapper;
using Slip.Service.Protos;

namespace Slip.Service.Events.Handlers;

public class UserRequestedBetPlacementHandler : IEventHandler<UserRequestedBetPlacement>
{
    private readonly ILogger<UserRequestedBetPlacementHandler> _logger;
    private readonly SlipContext _slipContext;
    private readonly IEnumerable<ISlipChain> _slipChain;
    public UserRequestedBetPlacementHandler(ILogger<UserRequestedBetPlacementHandler> logger, 
        SlipContext slipContext, 
        Event.EventClient eventGrpcClient, IEnumerable<ISlipChain> slipChain)
    {
        _logger = logger;
        _slipContext = slipContext;
        _slipChain = slipChain;
    }
    
    public async Task HandleAsync(UserRequestedBetPlacement @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("User {UserId} requested bet placement for Slip {SlipId}", @event.UserId, @event.Slip.Id);

        var slip = @event.Slip.ToDomain();
        
        foreach (var chain in _slipChain)
        {
            var chainResult = await chain.Handle(slip, cancellationToken);
            if (!chainResult)
            {
                _logger.LogInformation("Slip {SlipId} was not created", slip.Id);
                return;
            }
        }

        await _slipContext.Slips.AddAsync(slip, cancellationToken);
    }
    
    
}