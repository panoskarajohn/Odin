using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.SuspendMatch;

public record SuspendMatchCommand(long Id) : ICommand;