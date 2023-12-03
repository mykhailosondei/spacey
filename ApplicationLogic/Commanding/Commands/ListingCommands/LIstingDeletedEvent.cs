using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record ListingDeletedEvent : INotification
{
    public Guid ListingId { get; init; }
    public Guid HostId { get; init; }
    public List<Guid> BookingsIds { get; init; }
    public List<Guid> ReviewsIds { get; init; }
    public DateTime DeletedAt { get; init; }
}