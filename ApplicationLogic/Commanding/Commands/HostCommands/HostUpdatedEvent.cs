using MediatR;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record HostUpdatedEvent : INotification
{
    public Guid HostId { get; init; }
    public Guid UserId { get; init; }
    public DateTime UpdatedAt { get; init; }
}