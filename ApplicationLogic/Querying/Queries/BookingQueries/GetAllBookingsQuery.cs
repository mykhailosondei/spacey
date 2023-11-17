using ApplicationCommon.DTOs.Booking;
using MediatR;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetAllBookingsQuery() : IRequest<IEnumerable<BookingDTO>>;