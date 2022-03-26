using Shared.Cqrs.Commands;

namespace Event.Match.Features.CreateMatch;

public class CreateMatchCommandHandler : ICommandHandler<CreateMatchCommand>
{
    public Task HandleAsync(CreateMatchCommand command, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}