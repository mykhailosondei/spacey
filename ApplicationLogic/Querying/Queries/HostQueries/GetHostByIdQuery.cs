using ApplicationCommon.DTOs.Host;
using MediatR;

namespace ApplicationLogic.Querying.Queries.HostQueries;

public record GetHostByIdQuery(Guid Id) : IRequest<HostDTO>;