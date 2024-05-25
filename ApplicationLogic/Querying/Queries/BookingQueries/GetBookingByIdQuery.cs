using ApplicationCommon.DTOs.BookingDTOs;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetBookingByIdQuery(Guid Id) : IRequest<BookingDTO>;