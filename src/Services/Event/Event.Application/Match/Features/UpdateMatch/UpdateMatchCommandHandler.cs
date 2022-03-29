using Event.Application.Match.Exceptions;
using Event.Core.Enumerations;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.UpdateMatch;

public class UpdateMatchCommandHandler : ICommandHandler<UpdateMatchCommand>
{
    private readonly ILogger<UpdateMatchCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;

    public UpdateMatchCommandHandler(IMatchRepository matchRepository, ILogger<UpdateMatchCommandHandler> logger)
    {
        _matchRepository = matchRepository;
        _logger = logger;
    }

    public async Task HandleAsync(UpdateMatchCommand command, CancellationToken cancellationToken = default)
    {
        var match = await _matchRepository.Get(command.Id);

        if (match is null)
            throw new MatchNotFoundException(command.Id);

        match = Core.Models.Match.Create(command.Category,
            command.StartingTime,
            command.Home, command.Away,
            Status.Active.Name,
            command.Id);

        await _matchRepository.Update(match);
    }
}