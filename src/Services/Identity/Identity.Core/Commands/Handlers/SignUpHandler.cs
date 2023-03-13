using System.ComponentModel.DataAnnotations;
using Identity.Core.Entities;
using Identity.Core.Exceptions;
using Identity.Core.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Security;
using Shared.Web.Clock;

namespace Identity.Core.Commands.Handlers;

internal class SignUpHandler : ICommandHandler<SignUp>
{
    private static readonly string DefaultRole = Role.Default;
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();
    private readonly IUtcClock _clock;
    private readonly ILogger<SignUpHandler> _logger;
    private readonly IPasswordManager _passwordManager;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;

    public SignUpHandler(IUtcClock clock, IRoleRepository roleRepository, IUserRepository userRepository,
        IPasswordManager passwordManager, ILogger<SignUpHandler> logger)
    {
        _clock = clock;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _logger = logger;
    }

    public async Task HandleAsync(SignUp command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(command.Email) || !EmailAddressAttribute.IsValid(command.Email))
            throw new InvalidEmailException();

        if (string.IsNullOrEmpty(command.Password)) throw new InvalidPasswordException();

        var email = command.Email.ToLowerInvariant();
        var user = await _userRepository.GetAsync(email);

        if (user is not null) throw new EmailInUseException();

        var roleName = string.IsNullOrEmpty(command.Role) ? DefaultRole : command.Role.ToLowerInvariant();
        var role = await _roleRepository.GetAsync(roleName);

        if (role is null) throw new RoleNotFoundException();

        var now = _clock.Current();
        var password = _passwordManager.Secure(command.Password);
        user = new User
        {
            Id = command.UserId,
            Email = email,
            Password = password,
            Role = role,
            CreatedAt = now
        };

        await _userRepository.AddAsync(user);
    }
}