namespace Slip.Infrastructure.Repositories;

public class RedisKeys
{
    private const string SlipKey = "slip";
    public RedisKeys()
    {
    }

    public string BuildKey(string key) => $"{SlipKey}#{key}";
}