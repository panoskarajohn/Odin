using Shared.Cqrs.Commands;

namespace Event.Application.SportMatch.Features.CreateMatch;

public record CreateMatchCommand(string Category, DateTime StartingTime, string Home, string Away) : ICommand;