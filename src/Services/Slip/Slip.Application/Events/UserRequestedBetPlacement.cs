using Shared.Cqrs.Events;
using Shared.MessageBroker;

namespace Slip.Application.Events;

[Message("slip", "slip.created")]
public class UserRequestedBetPlacement : IEvent
{
    public UserRequestedBetPlacement(string userId, Core.Models.Slip slip)
    {
        UserId = userId;
        Slip = slip;
    }

    public string UserId { get; }
    public Core.Models.Slip Slip { get; }
}