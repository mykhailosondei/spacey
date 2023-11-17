using ApplicationCommon.DTOs.Booking;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Querying.Queries.BookingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.BookingHandlers;

public class GetBookingByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingByIdQuery, BookingDTO>
{
    private readonly IBookingQueryRepository _bookingQueryRepository;
    
    public GetBookingByIdQueryHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task<BookingDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookingQueryRepository.GetBookingById(request.Id);
        return _mapper.Map<BookingDTO>(result);
    }
}