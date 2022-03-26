using Event.Match.Repositories;
using Event.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;

namespace Event.Match.Features.CreateMatch;

public class CreateMatchCommandHandler : ICommandHandler<CreateMatchCommand>
{
    private readonly IMatchRepository _matchRepository;
    private readonly ILogger<CreateMatchCommandHandler> _logger;

    public CreateMatchCommandHandler(ILogger<CreateMatchCommandHandler> logger, IMatchRepository matchRepository)
    {
        _logger = logger;
        _matchRepository = matchRepository;
    }

    public async Task HandleAsync(CreateMatchCommand command, CancellationToken cancellationToken = default)
    {
        var match = Models.Match
            .Create(command.Category, 
                    command.StartingTime, 
                      command.Home, 
                      command.Away);
        
        await _matchRepository.Add(match);
    }
}