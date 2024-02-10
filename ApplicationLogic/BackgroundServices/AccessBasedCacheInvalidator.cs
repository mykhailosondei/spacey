using System.Text;
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Interfaces.QueryRepositories;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ApplicationLogic.BackgroundServices;

public class AccessBasedCacheInvalidator : BackgroundService, IDisposable
{
    private const int EXPIRATION_TIME_SECONDS = 5;
    private readonly IConnectionMultiplexer _redis;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly ILogger<AccessBasedCacheInvalidator> _logger;

    public AccessBasedCacheInvalidator(IConnectionMultiplexer redis, IMapper mapper, IServiceProvider serviceProvider, ILogger<AccessBasedCacheInvalidator> logger)
    {
        _redis = redis;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine(TimeSpan.FromSeconds(EXPIRATION_TIME_SECONDS));
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromSeconds(EXPIRATION_TIME_SECONDS));
        while (!stoppingToken.IsCancellationRequested)
        {
            await timer.WaitForNextTickAsync(stoppingToken);
            var server = _redis.GetServer(_redis.GetEndPoints()[0]);
            var keys = server.Keys(pattern: "*");
            _logger.LogInformation($"Checking {keys.Count()} keys...");
            _logger.LogInformation($"Keys: {string.Join(", ", keys)}");
            foreach (var key in keys)
            {
                string keyString = key.ToString();
                (string type, string guid) redisKey;
                var splittingIndex = keyString.IndexOf('-');
                redisKey.type = keyString.Substring(0, splittingIndex);
                redisKey.guid = keyString.Substring(splittingIndex + 1);
                if (redisKey.guid.Contains("timestamp"))
                {
                    continue;
                }
                _logger.LogInformation($"Checking {redisKey.type} {redisKey.guid} cache...");
                string cacheKeyTimestamp = $"{redisKey.type}-{redisKey.guid}-timestamp";
                string cachedTimestamp = await _redis.GetDatabase().HashGetAsync(cacheKeyTimestamp, "data");
                _logger.LogInformation($"Cached timestamp: {cachedTimestamp}");
                if (cachedTimestamp == null)
                {
                    _logger.LogInformation($"No timestamp found for {redisKey.type} {redisKey.guid} cache.");
                    await DeleteCache(key);
                    continue;
                }
                
                var cachedTimestampDateTime = JsonConvert.DeserializeObject<DateTime>(cachedTimestamp);
                if (IsEntityCacheExpired(cachedTimestampDateTime))
                {
                    _logger.LogInformation($"Cache for {redisKey.type} {redisKey.guid} is expired.");
                    await DeleteCache(key);
                    await DeleteCache(new RedisKey(cacheKeyTimestamp));
                }
                else
                {
                    _logger.LogInformation($"Cache for {redisKey.type} {redisKey.guid} is not expired.");
                }
            }
        }
    }
    
    private async Task DeleteCache(RedisKey key)
    {
        await _redis.GetDatabase().KeyDeleteAsync(key);
    }
    
    private bool IsEntityCacheExpired(DateTime lastCachedTime)
    {
        var now = DateTime.UtcNow;
        var timeSpan = now - lastCachedTime;
        return timeSpan.TotalSeconds > EXPIRATION_TIME_SECONDS;
    }
    
}