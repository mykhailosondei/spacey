using ApplicationCommon.DTOs.BookingDTOs;
using MediatR;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingDTO>>;