using Shared.Cqrs.Events;
using Shared.MessageBroker;

namespace Slip.Service.Events.Externals;

[Message("slip", "slip.created")]
public class UserRequestedBetPlacement : IEvent
{
    public string UserId { get; }
    public Domain.Slip Slip { get; }

    public UserRequestedBetPlacement(string userId , Domain.Slip slip)
    {
        UserId = userId;
        Slip = slip;
    }
}

