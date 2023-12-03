using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.HostCommands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicationLogic.NotificationHandlers;

public class CacheInvalidationHostHandler : INotificationHandler<HostUpdatedEvent>, INotificationHandler<HostDeletedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly IListingQueryRepository _listingQueryRepository;

    public CacheInvalidationHostHandler(IDistributedCache cache, IListingQueryRepository listingQueryRepository)
    {
        _cache = cache;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task Handle(HostUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"host-{notification.HostId}", CancellationToken.None);
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
    }

    public async Task Handle(HostDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"host-{notification.HostId}", CancellationToken.None);
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
        foreach (var id in notification.ListingsIds)
        {
            await _cache.RemoveAsync($"listing-{id}", CancellationToken.None);
            foreach (var bookingId in (await _listingQueryRepository.GetListingById(id)).BookingsIds)
            {
                await _cache.RemoveAsync($"booking-{bookingId}", CancellationToken.None);
            }
        }
        
    }
}