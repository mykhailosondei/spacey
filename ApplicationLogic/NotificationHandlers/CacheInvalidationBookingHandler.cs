using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicationLogic.NotificationHandlers;

internal class CacheInvalidationBookingHandler : INotificationHandler<BookingCreatedEvent>, INotificationHandler<BookingUpdatedEvent>, INotificationHandler<BookingDeletedEvent>
{
    private readonly IDistributedCache _cache;
    private readonly IBookingQueryRepository _bookingQueryRepository;

    public CacheInvalidationBookingHandler(IDistributedCache cache, IBookingQueryRepository bookingQueryRepository)
    {
        _cache = cache;
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync("bookings", CancellationToken.None);
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
        await _cache.RemoveAsync($"listing-{notification.ListingId}", CancellationToken.None);
    }

    public async Task Handle(BookingUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"booking-{notification.BookingId}", CancellationToken.None);
    }

    public async Task Handle(BookingDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"booking-{notification.BookingId}", CancellationToken.None);
        await _cache.RemoveAsync($"listing-{notification.ListingId}", CancellationToken.None);
        await _cache.RemoveAsync($"user-{notification.UserId}", CancellationToken.None);
    }
}