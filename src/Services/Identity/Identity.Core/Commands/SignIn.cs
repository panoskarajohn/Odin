using Shared.Cqrs.Commands;

namespace Identity.Core.Commands;

public record SignIn(string Email, string Password) : ICommand;