using Shared.Cqrs.Events;
using Shared.MessageBroker;
using Slip.Service.Contract;

namespace Slip.Service.Events.Externals;

[Message("slip", "slip.created")]
public class UserRequestedBetPlacement : IEvent
{
    public UserRequestedBetPlacement(string userId, SlipContract slip)
    {
        UserId = userId;
        Slip = slip;
    }

    public string UserId { get; }
    public SlipContract Slip { get; }
}