using Event.Application.SportMatch.Exceptions;
using Event.Core.DomainEvents;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Web.Context;

namespace Event.Application.SportMatch.Features.SuspendMatch;

public class SuspendMatchCommandHandler : ICommandHandler<SuspendMatchCommand>
{
    private readonly ILogger<SuspendMatchCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IContext _context;

    public SuspendMatchCommandHandler(IMatchRepository matchRepository, ILogger<SuspendMatchCommandHandler> logger, IContext context)
    {
        _matchRepository = matchRepository;
        _logger = logger;
        _context = context;
    }

    public async Task HandleAsync(SuspendMatchCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.Id);

        if (match is null)
            throw new MatchNotFoundException(command.Id);

        match.Suspend();
        match.AddDomainEvent(new MatchSuspendedEvent());

        await _matchRepository.Update(match, _context.Identity.Id.ToString());
    }
}