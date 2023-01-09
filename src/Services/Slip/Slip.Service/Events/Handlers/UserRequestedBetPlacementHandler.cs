using Shared.Cqrs.Events;
using Slip.Service.Events.Externals;

namespace Slip.Service.Events.Handlers;

public class UserRequestedBetPlacementHandler : IEventHandler<UserRequestedBetPlacement>
{
    private readonly ILogger<UserRequestedBetPlacementHandler> _logger;
    public UserRequestedBetPlacementHandler(ILogger<UserRequestedBetPlacementHandler> logger)
    {
        _logger = logger;
    }
    public Task HandleAsync(UserRequestedBetPlacement @event, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("User {UserId} requested bet placement for Slip {SlipId}", @event.UserId, @event.Slip.Id);
        return Task.CompletedTask;
    }
}