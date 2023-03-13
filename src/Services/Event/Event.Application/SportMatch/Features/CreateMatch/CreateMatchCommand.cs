using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.CreateMatch;

public record CreateMatchCommand
    (long EventId, string Category, DateTime StartingTime, string Home, string Away) : ICommand;