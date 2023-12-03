using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record ListingCreatedEvent : INotification
{
    public Guid ListingId { get; init; }
    public Guid HostId { get; init; }
    public DateTime CreatedAt { get; init; }
}