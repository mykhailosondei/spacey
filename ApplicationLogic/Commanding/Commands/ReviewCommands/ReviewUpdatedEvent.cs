using MediatR;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record ReviewUpdatedEvent : INotification
{
    public Guid ReviewId { get; init; }
    public Guid UserId { get; init; }
    public Guid BookingId { get; init; }
    public DateTime UpdatedAt { get; init; }
}