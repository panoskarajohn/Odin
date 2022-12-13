using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Web.Clock;


namespace Shared.Jwt;

internal sealed class JsonWebTokenManager : IJsonWebTokenManager
{
    private static readonly Dictionary<string, IEnumerable<string>> EmptyClaims = new();

    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    private readonly string? _issuer;

    private readonly TimeSpan _expiry;

    private readonly IUtcClock _clock;
    private readonly SigningCredentials _signingCredentials;
    private readonly string? _audience;

    public JsonWebTokenManager(IOptions<AuthOptions> authOptions,IUtcClock clock, SecurityKeyDetails securityKeyDetails)
    {
        Guard.Against.Null(authOptions, nameof(authOptions));
        _clock = Guard.Against.Null(clock, nameof(clock));
        _audience = authOptions.Value.Jwt.Authority;
        _issuer = authOptions.Value.Jwt.Issuer;
        _expiry = authOptions.Value.Jwt.Expiry ?? TimeSpan.FromHours(1);
        _signingCredentials = new SigningCredentials(securityKeyDetails.Key, securityKeyDetails.Algorithm);
    }
    
    public JsonWebToken CreateToken(string userId, string? email = null, string? role = null, IDictionary<string, IEnumerable<string>>? claims = null)
    {
        var now = _clock.Current();

        var jwtClaims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sub, userId),
            new(JwtRegisteredClaimNames.UniqueName, userId)
        };

        if (!string.IsNullOrEmpty(email))
        {
            jwtClaims.Add(new(JwtRegisteredClaimNames.Email, email));
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            jwtClaims.Add(new(ClaimTypes.Role, role));
        }
        
        if (!string.IsNullOrWhiteSpace(_audience))
        {
            jwtClaims.Add(new Claim(JwtRegisteredClaimNames.Aud, _audience));
        }
        
        if (claims?.Any() is true)
        {
            var customClaims = new List<Claim>();
            foreach (var (claim, values) in claims)
            {
                customClaims.AddRange(values.Select(value => new Claim(claim, value)));
            }

            jwtClaims.AddRange(customClaims);
        }
        
        var expires = now.Add(_expiry);

        var jwt = new JwtSecurityToken(
            _issuer,
            claims: jwtClaims,
            notBefore: now,
            expires: expires,
            signingCredentials: _signingCredentials
        );
        
        var token = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new JsonWebToken
        {
            AccessToken = token,
            Expiry = new DateTimeOffset(expires).ToUnixTimeMilliseconds(),
            UserId = userId,
            Email = email ?? string.Empty,
            Role = role ?? string.Empty,
            Claims = claims ?? EmptyClaims
        };
        
    }
}