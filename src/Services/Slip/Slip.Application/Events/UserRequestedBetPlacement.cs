using Shared.Cqrs.Events;
using Shared.MessageBroker;
using System;

namespace Slip.Application.Events;

[Message("slip", "slip.created")]
public class UserRequestedBetPlacement : IEvent
{
    public string UserId { get; }
    public Core.Models.Slip Slip { get; }

    public UserRequestedBetPlacement(string userId ,Core.Models.Slip slip)
    {
        UserId = userId;
        Slip = slip;
    }
}