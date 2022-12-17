using Microsoft.IdentityModel.JsonWebTokens;

namespace Identity.Core.Services;

public interface ITokenStorage
{
    void Set(JsonWebToken jwt);
    JsonWebToken? Get();
}