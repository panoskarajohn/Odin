using Event.Application.SportMatch.Exceptions;
using Event.Core.DomainEvents;
using Event.Core.Enumerations;
using Event.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Web.Context;

namespace Event.Application.SportMatch.Features.UpdateMatch;

public class UpdateMatchCommandHandler : ICommandHandler<UpdateMatchCommand>
{
    private readonly ILogger<UpdateMatchCommandHandler> _logger;
    private readonly IMatchRepository _matchRepository;
    private readonly IContext _context;

    public UpdateMatchCommandHandler(IMatchRepository matchRepository, ILogger<UpdateMatchCommandHandler> logger, IContext context)
    {
        _matchRepository = matchRepository;
        _logger = logger;
        _context = context;
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

        match.AddDomainEvent(new UpdatedEventMatch());

        await _matchRepository.Update(match, _context.Identity.Id.ToString());
    }
}