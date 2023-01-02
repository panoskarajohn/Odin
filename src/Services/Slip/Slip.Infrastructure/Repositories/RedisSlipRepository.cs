using System.Text.Json;
using Microsoft.Extensions.Logging;
using Slip.Core.Repositories;
using StackExchange.Redis;

namespace Slip.Infrastructure.Repositories;

public class RedisSlipRepository : ISlipRepository
{
    private readonly ILogger<RedisSlipRepository> _logger;
    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    
    public RedisSlipRepository(ILogger<RedisSlipRepository> logger, ConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
        _database = _redis.GetDatabase();
    }
    
    public async Task<Core.Models.Slip> GetSlipAsync(string userId, CancellationToken cancellationToken)
    {
        var data = await _database.StringGetAsync(userId);

        if (data.IsNullOrEmpty)
        {
            return null;
        }

        return JsonSerializer.Deserialize<Core.Models.Slip>(data, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<Core.Models.Slip> UpdateSlipAsync(Core.Models.Slip slip, CancellationToken cancellationToken)
    {
        var created = await _database.StringSetAsync(slip.UserId.ToString(), JsonSerializer.Serialize(slip));

        if (!created)
        {
            _logger.LogInformation("Problem occur persisting the slip");
            return null;
        }

        _logger.LogInformation("Slip persisted successfully in the database");

        return await GetSlipAsync(slip.UserId.ToString(), cancellationToken);
    }

    public async Task<bool> DeleteSlipAsync(string userId, CancellationToken cancellationToken)
    {
        return await _database.KeyDeleteAsync(userId);
    }
    
    private IServer GetServer()
    {
        var endpoint = _redis.GetEndPoints();
        return _redis.GetServer(endpoint.First());
    }
}