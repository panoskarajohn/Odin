using Shared.Cqrs.Commands;
using Shared.Web.Context;
using Slip.Application.Exceptions;
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

        if (!betsDto.Any())
            throw new NoBetsException();
        
        foreach (var betDto in betsDto)
        {
            var bet = Bet.Create()
                .WithStake(betDto.Stake);

            foreach (var betSelectionDto in betDto.Selections)
            {
                var betSelection =  BetSelection.Create(betSelectionDto.EventId, betSelectionDto.MarketName, betSelectionDto.Outcome, betSelectionDto.Odds);
                bet.AddSelection(betSelection);
            }
            
            slip.AddBet(bet);
        }

        await _slipRepository.UpdateSlipAsync(slip, cancellationToken);
    }
}