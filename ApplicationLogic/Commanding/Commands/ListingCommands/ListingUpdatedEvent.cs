using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record ListingUpdatedEvent : INotification
{
    public Guid ListingId { get; init; }
    public Guid HostId { get; init; }
    public DateTime UpdatedAt { get; init; }
}