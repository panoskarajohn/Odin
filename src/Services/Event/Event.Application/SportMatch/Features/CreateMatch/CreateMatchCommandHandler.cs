using Event.Core.DomainEvents;
using Event.Core.Repositories;
using Event.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Web.Context;

namespace Event.Application.SportMatch.Features.CreateMatch;

public class CreateMatchCommandHandler : ICommandHandler<CreateMatchCommand>
{
    private readonly ILogger<CreateMatchCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IContext _context;

    public CreateMatchCommandHandler(ILogger<CreateMatchCommandHandler> logger, IMatchRepository matchRepository, IContext context)
    {
        _logger = logger;
        _matchRepository = matchRepository;
        _context = context;
    }

    public async Task HandleAsync(CreateMatchCommand command, CancellationToken cancellationToken = default)
    {
        var _ = new StartingTime(command.StartingTime);
        var match = Core.Models.Match.Create(command.Category,
            command.StartingTime,
            command.Home,
            command.Away);

        match.AddDomainEvent(new NewMatchCreatedEvent());

        await _matchRepository.Add(match, _context.Identity.Id.ToString());
    }
}