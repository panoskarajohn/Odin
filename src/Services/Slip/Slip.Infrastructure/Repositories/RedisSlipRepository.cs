using System.Text.Json;
using Microsoft.Extensions.Logging;
using Slip.Core.Repositories;
using Slip.Infrastructure.RedisDtos;
using StackExchange.Redis;

namespace Slip.Infrastructure.Repositories;

public class RedisSlipRepository : ISlipRepository
{
    private readonly ILogger<RedisSlipRepository> _logger;
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private readonly RedisKeys _redisKeys;

    public RedisSlipRepository(ILogger<RedisSlipRepository> logger, IConnectionMultiplexer redis, RedisKeys redisKey)
    {
        _logger = logger;
        _redis = redis;
        _database = _redis.GetDatabase();
        _redisKeys = redisKey;
    }
    
    public async Task<Core.Models.Slip> GetSlipAsync(string userId, CancellationToken cancellationToken)
    {
        var key = _redisKeys.BuildSlipKey(userId);
        var data = await _database.StringGetAsync(key);

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        var dto = JsonSerializer.Deserialize<RedisDtos.Slip>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return dto.ToDomain();
    }

    public async Task UpdateSlipAsync(Core.Models.Slip slip, CancellationToken cancellationToken)
    {
        var key = _redisKeys.BuildSlipKey(slip.UserId);
        var serialized = JsonSerializer.Serialize(slip);
        var created = await _database.StringSetAsync(key, serialized, TimeSpan.FromHours(2));

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the slip");
            return;
        }

        _logger.LogInformation("Slip persisted successfully in the database");
    }

    public async Task<bool> DeleteSlipAsync(string userId, CancellationToken cancellationToken)
    {
        var key = _redisKeys.BuildSlipKey(userId);
        _logger.LogInformation("Deleting slip with key {Key}", key);
        return await _database.KeyDeleteAsync(key);
    }
    
    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints();
        return _redis.GetServer(endpoint.First());
    }
}