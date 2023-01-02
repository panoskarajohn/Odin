using Shared.Cqrs.Commands;
using Shared.Web.Context;
using Slip.Core.Repositories;
using Slip.Core.ValueObjects;

namespace Slip.Application.Commands.BuildSlip;

public class BuildSlipCommandHandler : ICommandHandler<BuildSlipCommand>
{
    private readonly IContext _context;
    private readonly ISlipRepository _slipRepository;
    
    public BuildSlipCommandHandler(IContext context, ISlipRepository slipRepository)
    {
        _context = context;
        _slipRepository = slipRepository;
    }
    
    public async Task HandleAsync(BuildSlipCommand command, CancellationToken cancellationToken = default)
    {
        var userId = _context.Identity.Id;

        var slip = Core.Models.Slip.Create(userId);

        var betsDto = command.Bets;
        
        foreach (var betDto in betsDto)
        {
            var bet = new Bet();
            foreach (var betSelectionDto in betDto.Selections)
            {
                var betSelection = new BetSelection(betSelectionDto.EventId, betSelectionDto.MarketName, betSelectionDto.Outcome, betSelectionDto.Odds);
                bet.AddSelection(betSelection);
            }
            
            slip.AddBet(bet);
        }

        await _slipRepository.UpdateSlipAsync(slip, cancellationToken);
    }
}