using Shared.Cqrs.Commands;

namespace Identity.Core.Commands;

public record SignUp(string Email, string Password, string? Role = null) : ICommand
{
    public Guid UserId { get; init; } = Guid.NewGuid();
}