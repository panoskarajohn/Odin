using Microsoft.AspNetCore.Identity;

namespace Shared.Security.Encryption;

internal class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<object> _passwordHasher;
    public PasswordManager(IPasswordHasher<object> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    public string Secure(string password)
    {
        return _passwordHasher.HashPassword(new object(),password);
    }

    public bool IsValid(string password, string securedPassword)
    {
        return _passwordHasher
                   .VerifyHashedPassword(new object(), securedPassword, password) 
               == PasswordVerificationResult.Success;
    }
}