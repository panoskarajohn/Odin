using System.Net;
using System.Net.Http.Headers;
using Ardalis.GuardClauses;
using IdentityModel.Client;

namespace Backoffice;

public class AuthenticationDelegateHandler : DelegatingHandler
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ClientCredentialsTokenRequest _tokenRequest;
    private ILogger<AuthenticationDelegateHandler> _logger;


    public AuthenticationDelegateHandler(IHttpClientFactory clientFactory, ClientCredentialsTokenRequest tokenRequest, ILogger<AuthenticationDelegateHandler> logger)
    {
        _logger = Guard.Against.Null(logger);
        _clientFactory = Guard.Against.Null(clientFactory);
        _tokenRequest = Guard.Against.Null(tokenRequest);
    }
    
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var client = _clientFactory.CreateClient("IDPClient");
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(_tokenRequest, cancellationToken);

        if (tokenResponse.IsError)
        {
            _logger.LogCritical("Error getting token from IDP: {error}", tokenResponse.Error);
            return new HttpResponseMessage(HttpStatusCode.InternalServerError);
        }
        
        _logger.LogInformation("Token received from IDP: {token}", tokenResponse.AccessToken);
        request.SetBearerToken(tokenResponse.AccessToken);
        return await base.SendAsync(request, cancellationToken);
    }
}