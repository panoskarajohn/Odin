using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.UpdateMatch;

public record UpdateMatchCommand(long Id, string Category, DateTime StartingTime, string Home, string Away) : ICommand;