using Shared.Cqrs.Commands;

namespace Event.Application.Match.Features.CreateMatch;

public record CreateMatchCommand(string Category, DateTime StartingTime, string Home, string Away) : ICommand;