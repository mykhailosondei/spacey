using ApplicationLogic.Commanding.Commands.BookingCommands;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace ApplicationLogic.NotificationHandlers;

internal class CacheInvalidationBookingHandler : INotificationHandler<BookingCreatedEvent>, INotificationHandler<BookingUpdatedEvent>, INotificationHandler<BookingDeletedEvent>
{
    private readonly IDistributedCache _cache;

    public CacheInvalidationBookingHandler(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync("bookings", CancellationToken.None);
    }

    public async Task Handle(BookingUpdatedEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Booking updated: {notification.BookingId}");
        await _cache.RemoveAsync($"booking-{notification.BookingId}", CancellationToken.None);
    }

    public async Task Handle(BookingDeletedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync($"booking-{notification.BookingId}", CancellationToken.None);
    }
}