namespace Slip.Infrastructure.Repositories;

public class RedisKeys
{
    private const string SlipKey = "slip";

    public string BuildKey(string key)
    {
        return $"{SlipKey}#{key}";
    }
}