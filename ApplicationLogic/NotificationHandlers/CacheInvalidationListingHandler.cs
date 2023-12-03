using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicationLogic.NotificationHandlers;

public class CacheInvalidationListingHandler : INotificationHandler<ListingUpdatedEvent>,
    INotificationHandler<ListingDeletedEvent>, INotificationHandler<ListingCreatedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly IBookingQueryRepository _bookingQueryRepository;

    public CacheInvalidationListingHandler(IDistributedCache cache, IBookingQueryRepository bookingQueryRepository)
    {
        _cache = cache;
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task Handle(ListingUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"listing-{notification.ListingId}", CancellationToken.None);
    }

    public async Task Handle(ListingDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"listing-{notification.ListingId}", CancellationToken.None);
        await _cache.RemoveAsync($"host-{notification.HostId}", CancellationToken.None);
        foreach (var id in notification.BookingsIds)
        {
            await _cache.RemoveAsync($"booking-{id}", CancellationToken.None);
        }
    }

    public async Task Handle(ListingCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"host-{notification.HostId}", CancellationToken.None);
    }
}