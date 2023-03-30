using System.Security.Claims;

namespace Shared.Web.Context;

public class IdentityContext : IIdentityContext
{
    private IdentityContext()
    {
    }

    public IdentityContext(Guid? id)
    {
        Id = id ?? Guid.Empty;
        IsAuthenticated = id.HasValue;
    }

    public IdentityContext(ClaimsPrincipal principal)
    {
        if (principal?.Identity is null || string.IsNullOrWhiteSpace(principal.Identity.Name)) return;

        IsAuthenticated = principal.Identity?.IsAuthenticated is true;
        Id = IsAuthenticated ? Guid.Parse(principal.Identity.Name) : Guid.Empty;
        Role = principal.Claims.SingleOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
        Claims = principal.Claims.GroupBy(x => x.Type)
            .ToDictionary(x => x.Key, x => x.Select(c => c.Value.ToString()));
    }

    public static IIdentityContext Empty => new IdentityContext();
    public bool IsAuthenticated { get; }
    public Guid Id { get; }
    public string Role { get; }

    public Dictionary<string, IEnumerable<string>> Claims { get; }

    public bool IsUser()
    {
        return Role is "user";
    }

    public bool IsAdmin()
    {
        return Role is "admin";
    }
}