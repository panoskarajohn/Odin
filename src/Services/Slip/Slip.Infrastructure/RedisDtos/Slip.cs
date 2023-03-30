using Slip.Core.ValueObjects;

namespace Slip.Infrastructure.RedisDtos;

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

public static class RedistMapping
{
    public static Core.Models.Slip ToDomain(this Slip redisSlip)
    {
        var slip = Core.Models.Slip.Create(Guid.Parse(redisSlip.UserId), Guid.Parse(redisSlip.Id));

        foreach (var redisSlipBet in redisSlip.Bets)
        {
            var bet = Core.ValueObjects.Bet.Create();
            bet.WithStake(redisSlipBet.Stake);

            foreach (var betSelection in redisSlipBet.Selections)
            {
                var selection = BetSelection.Create(betSelection.EventId, betSelection.MarketName, betSelection.Outcome,
                    betSelection.Odds);
                bet.AddSelection(selection);
            }

            slip.AddBet(bet);
        }

        return slip;
    }
}