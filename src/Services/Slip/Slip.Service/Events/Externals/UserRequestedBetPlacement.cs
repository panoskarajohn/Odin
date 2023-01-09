using Shared.Cqrs.Events;
using Shared.MessageBroker;

namespace Slip.Service.Events.Externals;

[Message("slip", "slip.created")]
public class UserRequestedBetPlacement : IEvent
{
    public string UserId { get; }
    public Slip Slip { get; }

    public UserRequestedBetPlacement(string userId , Slip slip)
    {
        UserId = userId;
        Slip = slip;
    }
}

public class Slip
{
    public string Id { get; set; }
    public List<Bet> Bets { get; set; }
    public string UserId { get; set; }
    public decimal TotalStake { get; set; }
}

public class Bet
{
    public List<Selection> Selections { get; set; }
    public decimal Stake { get; set; }
    public string BetType { get; set; }
    public decimal Winnings { get; set; }
    public int NumberOfSelections { get; set; }
}

public class Selection
{
    public long EventId { get; set; }
    public string MarketName { get; set; }
    public string Outcome { get; set; }
    public decimal Odds { get; set; }
}