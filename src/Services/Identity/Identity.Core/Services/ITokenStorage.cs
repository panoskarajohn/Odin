using Shared.Jwt;

namespace Identity.Core.Services;

public interface ITokenStorage
{
    void Set(JsonWebToken jwt);
    JsonWebToken? Get();
}