using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record BookingDeletedEvent : INotification
{
    public Guid BookingId { get; init; }
    public Guid UserId { get; init; }
    public Guid ListingId { get; init; }
    public DateTime DeletedAt { get; init; }
}