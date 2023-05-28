namespace Slip.Infrastructure.Repositories;

public class RedisKeys
{
    private const string SlipKey = "slip";
    public RedisKeys()
    {
    }

    public string BuildSlipKey(string key) => $"{SlipKey}#{key}";
}