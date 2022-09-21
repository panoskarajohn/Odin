using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.UpdateMatch;

public record UpdateMatchCommand(long Id, string Category, DateTime StartingTime, string Home, string Away) : ICommand;