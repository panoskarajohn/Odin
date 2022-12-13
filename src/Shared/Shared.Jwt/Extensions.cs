using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Shared.Common;

namespace Shared.Jwt;

public static class Extensions
{
    public static IServiceCollection AddJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetOptions<JwtBearerOptions>("Jwt");
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,options =>
            {
                options.Authority = jwtOptions.Authority;
                options.TokenValidationParameters.ValidateAudience = false;
            });

        if (!string.IsNullOrEmpty(jwtOptions.Audience))
        {
            services.AddAuthorization(option =>
            {
                option.AddPolicy(nameof(ApiScope), policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", jwtOptions.Audience);
                });
            });
        }
        
        return services;
    }
    
}