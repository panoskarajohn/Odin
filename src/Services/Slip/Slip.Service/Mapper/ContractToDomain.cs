using Slip.Service.Contract;
using Slip.Service.Domain;

namespace Slip.Service.Mapper;

public static class ContractToDomain
{
    public static Domain.Slip ToDomain(this SlipContract contract)
    {
        return new Domain.Slip
        {
            Id = contract.Id,
            UserId = contract.UserId,
            TotalStake = contract.TotalStake,
            Bets = contract.Bets.Select(b => b.ToDomain()).ToList()
        };
    }

    private static Bet ToDomain(this BetContract contract)
    {
        return new Bet
        {
            Id = contract.Id,
            Stake = contract.Stake,
            BetType = contract.BetType,
            Winnings = contract.Winnings,
            Selections = contract.Selections.Select(s => s.ToDomain()).ToList(),
            NumberOfSelections = contract.Selections.Count,
            BetStatus = contract.BetStatus
        };
    }

    private static BetSelection ToDomain(this BetSelectionContract contract)
    {
        return new BetSelection
        {
            Id = contract.Id,
            EventId = contract.EventId,
            MarketName = contract.MarketName,
            Outcome = contract.Outcome,
            Odds = contract.Odds
        };
    }
}