using ApplicationCommon.DTOs.Booking;
using MediatR;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDTO>;