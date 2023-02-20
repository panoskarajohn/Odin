using System.ComponentModel.Design;

namespace Slip.Service.Mapper;

public static class ContractToDomain
{
    public static Domain.Slip ToDomain(this Contract.SlipContract contract)
    {
        return new Domain.Slip
        {
            Id = contract.Id,
            UserId = contract.UserId,
            TotalStake = contract.TotalStake,
            Bets = contract.Bets.Select(b => b.ToDomain()).ToList()
        };
    }

    private static Domain.Bet ToDomain(this Contract.BetContract contract)
    {
        return new Domain.Bet
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

    private static Domain.BetSelection ToDomain(this Contract.BetSelectionContract contract)
    {
        return new Domain.BetSelection
        {
            Id = contract.Id,
            EventId = contract.EventId,
            MarketName = contract.MarketName,
            Outcome = contract.Outcome,
            Odds = contract.Odds,
        };
    }
}