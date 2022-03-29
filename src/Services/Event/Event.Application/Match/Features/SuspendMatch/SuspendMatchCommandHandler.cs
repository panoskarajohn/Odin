using Event.Application.Match.Exceptions;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.SuspendMatch;

public class SuspendMatchCommandHandler : ICommandHandler<SuspendMatchCommand>
{
    private readonly ILogger<SuspendMatchCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;

    public SuspendMatchCommandHandler(IMatchRepository matchRepository, ILogger<SuspendMatchCommandHandler> logger)
    {
        _matchRepository = matchRepository;
        _logger = logger;
    }

    public async Task HandleAsync(SuspendMatchCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.Id);

        if (match is null)
            throw new MatchNotFoundException(command.Id);

        match.Suspend();
        await _matchRepository.Update(match);
    }
}