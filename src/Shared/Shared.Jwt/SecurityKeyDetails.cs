using Microsoft.IdentityModel.Tokens;

namespace Shared.Jwt;

internal sealed record SecurityKeyDetails(SecurityKey Key, string Algorithm);