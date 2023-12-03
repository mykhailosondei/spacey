using MediatR;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record ReviewCreatedEvent : INotification
{
    public Guid ReviewId { get; init; }
    public Guid UserId { get; init; }
    public Guid BookingId { get; init; }
    public DateTime CreatedAt { get; init; }
}