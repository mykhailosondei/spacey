using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.UserCommands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicationLogic.NotificationHandlers;

public class CacheInvalidationUserHandler : INotificationHandler<UserUpdatedEvent>, INotificationHandler<UserDeletedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly IListingQueryRepository _listingQueryRepository;

    public CacheInvalidationUserHandler(IDistributedCache cache, IListingQueryRepository listingQueryRepository)
    {
        _cache = cache;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task Handle(UserUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
    }

    public async Task Handle(UserDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
        await _cache.RemoveAsync($"host-{notification.HostId}", CancellationToken.None);
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