using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record BookingCreatedEvent : INotification
{
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public Guid ListingId { get; init; }
    public DateTime CreatedAt { get; init; }
}