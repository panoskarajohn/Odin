using System.ComponentModel.DataAnnotations;
using Identity.Core.Exceptions;
using Identity.Core.Repositories;
using Identity.Core.Services;
using Microsoft.Extensions.Logging;
using Shared.Cqrs.Commands;
using Shared.Jwt;
using Shared.Security;

namespace Identity.Core.Commands.Handlers;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    private static readonly EmailAddressAttribute EmailAddressAttribute = new();
    private readonly IJsonWebTokenManager _jsonWebTokenManager;
    private readonly ILogger<SignInHandler> _logger;
    private readonly IPasswordManager _passwordManager;
    private readonly ITokenStorage _tokenStorage;
    private readonly IUserRepository _userRepository;

    public SignInHandler(IUserRepository userRepository, IJsonWebTokenManager jsonWebTokenManager,
        IPasswordManager passwordManager, ITokenStorage tokenStorage,
        ILogger<SignInHandler> logger)
    {
        _userRepository = userRepository;
        _jsonWebTokenManager = jsonWebTokenManager;
        _passwordManager = passwordManager;
        _tokenStorage = tokenStorage;
        _logger = logger;
    }

    public async Task HandleAsync(SignIn command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.Email) || !EmailAddressAttribute.IsValid(command.Email))
            throw new InvalidEmailException();

        if (string.IsNullOrWhiteSpace(command.Password)) throw new MissingPasswordException();

        var user = await _userRepository.GetAsync(command.Email.ToLowerInvariant());
        if (user is null) throw new InvalidCredentialsException();

        if (!_passwordManager.IsValid(command.Password, user.Password)) throw new InvalidCredentialsException();

        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["permissions"] = user.Role.Permissions
        };

        var jwt = _jsonWebTokenManager.CreateToken(user.Id.ToString(), user.Email, user.Role.Name, claims);
        jwt.Email = user.Email;
        _logger.LogInformation($"User with ID: '{user.Id}' has signed in.");
        _tokenStorage.Set(jwt);
    }
}