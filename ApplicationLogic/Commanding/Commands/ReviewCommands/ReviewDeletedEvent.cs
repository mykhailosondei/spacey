using MediatR;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record ReviewDeletedEvent : INotification
{
    public Guid ReviewId { get; init; }
    public Guid UserId { get; init; }
    public Guid BookingId { get; init; }
    public DateTime DeletedAt { get; init; }
}