using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Security.Encryption;
using Shared.Security.Random;
using Shared.Security.Signing;

namespace Shared.Security;

public static class Extensions
{
    private const string SectionName = "security";

    public static IServiceCollection AddOdinSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(SectionName);
        services.Configure<SecurityOptions>(section);

        services
            .AddSingleton<IEncryptor, AesEncryptor>()
            .AddSingleton<IShaHasher, ShaHasher>()
            .AddSingleton<IRng, Rng>()
            .AddSingleton<ISigner, Signer>()
            .AddSingleton<IPasswordManager, PasswordManager>()
            .AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();

        return services;
    }
}