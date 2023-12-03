using ApplicationDAL.Entities;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.UserCommands;

public record UserDeletedEvent : INotification
{
    public Guid UserId { get; init; }
    public Guid HostId { get; init; }
    public List<Guid> BookingsIds { get; init; }
    public List<Guid> ListingsIds { get; init; }
    public DateTime DeletedAt { get; init; }
}