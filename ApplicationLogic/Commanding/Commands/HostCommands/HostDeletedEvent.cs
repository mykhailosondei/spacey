using MediatR;

namespace ApplicationLogic.Commanding.Commands.HostCommands;

public record HostDeletedEvent : INotification
{
    public Guid HostId { get; init; }
    public Guid UserId { get; init; }
    public List<Guid> ListingsIds { get; init; }
    public List<Guid> BookingsIds { get; init; }
    public DateTime DeletedAt { get; init; }
}