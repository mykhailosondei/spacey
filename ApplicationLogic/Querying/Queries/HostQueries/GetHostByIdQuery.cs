using ApplicationCommon.DTOs.Host;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.HostQueries;

public record GetHostByIdQuery(Guid Id) : IRequest<HostDTO>;