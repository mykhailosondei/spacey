using ApplicationCommon.DTOs.BookingDTOs;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingDTO>>;