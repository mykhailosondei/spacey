using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record BookingUpdatedEvent : INotification
{
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public Guid ListingId { get; init; }
    public DateTime UpdatedAt { get; init; }
}