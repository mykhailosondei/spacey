using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.Enums;
using CustomMediator;

namespace ApplicationLogic.Querying.Queries.BookingQueries;

public record GetBookingsByStatusQuery(BookingStatus Status) : IRequest<IEnumerable<BookingDTO>>;