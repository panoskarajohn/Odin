using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace Shared.Jwt;

public class AuthHeaderHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public AuthHeaderHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var token = (_contextAccessor?.HttpContext?.Request.Headers["Authorization"])?.ToString();

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token?.Replace("Bearer ", "", StringComparison.Ordinal));

        return base.SendAsync(request, cancellationToken);
    }
}