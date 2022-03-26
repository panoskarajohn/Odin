using Shared.Cqrs.Commands;

namespace Event.Match.Features.CreateMatch;

public record CreateMatchCommand(string Category, DateTime StartingTime, string Home, string Away) : ICommand;