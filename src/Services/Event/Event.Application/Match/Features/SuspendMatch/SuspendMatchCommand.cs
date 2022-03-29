using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.SuspendMatch;

public record SuspendMatchCommand(long Id) : ICommand;