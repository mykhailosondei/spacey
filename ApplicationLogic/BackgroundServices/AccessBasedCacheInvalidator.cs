using System.Text;
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.DTOs.User;
using ApplicationCommon.Interfaces;
using ApplicationDAL.Interfaces.QueryRepositories;
using AutoMapper;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;

namespace ApplicationLogic.BackgroundServices;

public class AccessBasedCacheInvalidator : BackgroundService, IDisposable
{
    private const int EXPIRATION_TIME_MINUTES = 5;
    private readonly IConnectionMultiplexer _redis;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IMapper _mapper;

    public AccessBasedCacheInvalidator(IBookingQueryRepository bookingQueryRepository, IListingQueryRepository listingQueryRepository, IUserQueryRepository userQueryRepository, IHostQueryRepository hostQueryRepository, IConnectionMultiplexer redis, IMapper mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
        _listingQueryRepository = listingQueryRepository;
        _userQueryRepository = userQueryRepository;
        _hostQueryRepository = hostQueryRepository;
        _redis = redis;
        _mapper = mapper;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var keys = _redis.GetServer(_redis.GetEndPoints()[0]).Keys(pattern: "*");
        foreach (var key in keys)
        {
            string keyString = key.ToString();
            (string type, string guid) redisKey;
            var splittingIndex = keyString.IndexOf('-');
            redisKey.type = keyString.Substring(0, splittingIndex);
            redisKey.guid = keyString.Substring(splittingIndex + 2);
            switch (redisKey.type)
            {
                case "booking":
                    var booking = await _bookingQueryRepository.GetBookingById(Guid.Parse(redisKey.guid));
                    var bookingDTO = _mapper.Map<BookingDTO>(booking);
                    if (IsEntityCacheExpired(bookingDTO))
                    {
                        await DeleteCache(key);
                    }
                    break;
                case "listing":
                    var listing = await _listingQueryRepository.GetListingById(Guid.Parse(redisKey.guid));
                    var listingDTO = _mapper.Map<ListingDTO>(listing);
                    if (IsEntityCacheExpired(listingDTO))
                    {
                        await DeleteCache(key);
                    }
                    break;
                case "user":
                    var user = await _userQueryRepository.GetUserById(Guid.Parse(redisKey.guid));
                    var userDTO = _mapper.Map<UserDTO>(user);
                    if (IsEntityCacheExpired(userDTO))
                    {
                        await DeleteCache(key);
                    }
                    break;
                case "host":
                    var host = await _hostQueryRepository.GetHostById(Guid.Parse(redisKey.guid));
                    var hostDTO = _mapper.Map<HostDTO>(host);
                    if (IsEntityCacheExpired(hostDTO))
                    {
                        await DeleteCache(key);
                    }
                    break;
            }
        }
        
        await Task.Delay(TimeSpan.FromMinutes(EXPIRATION_TIME_MINUTES), stoppingToken);
    }
    
    private async Task DeleteCache(RedisKey key)
    {
        await _redis.GetDatabase().KeyDeleteAsync(key);
    }
    
    private bool IsEntityCacheExpired(ILastAccessible entity)
    {
        var lastAccess = entity.LastAccess;
        var now = DateTime.Now;
        var timeSpan = now - lastAccess;
        return timeSpan.TotalMinutes > EXPIRATION_TIME_MINUTES;
    }
    
}