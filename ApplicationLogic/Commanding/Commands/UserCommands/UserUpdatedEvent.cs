using MediatR;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record UserUpdatedEvent : INotification
{
    public Guid UserId { get; init; }
    public DateTime UpdatedAt { get; init; }
}