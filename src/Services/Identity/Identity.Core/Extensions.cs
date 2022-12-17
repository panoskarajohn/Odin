using Identity.Core.DAL;
using Identity.Core.DAL.Repositories;
using Identity.Core.Entities;
using Identity.Core.Repositories;
using Identity.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.DAL.Postgres;

namespace Identity.Core;

public static class Extensions
{
    public static IServiceCollection AddIdentityCore(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<ITokenStorage, HttpContextTokenStorage>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddScoped<IRoleRepository, RoleRepository>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddPostgres<UsersDbContext>(configuration);
    }
}