using Shared.Cqrs.Commands;

namespace Slip.Application.Commands.BuildSlip;

public record BuildSlipCommand(IEnumerable<BetDto> Bets) : ICommand;

public record BetDto(IEnumerable<BetSelectionDto> Selections);

public record BetSelectionDto(long EventId, string MarketName, string Outcome, decimal Odds);