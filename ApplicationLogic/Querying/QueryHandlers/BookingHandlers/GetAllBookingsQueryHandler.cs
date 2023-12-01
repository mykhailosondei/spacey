using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.BookingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.BookingHandlers;

public class GetAllBookingsQueryHandler : BaseHandler, IRequestHandler<GetAllBookingsQuery, IEnumerable<BookingDTO>>
{
    private readonly IBookingQueryRepository _bookingQueryRepository;
    
    public GetAllBookingsQueryHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task<IEnumerable<BookingDTO>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookingQueryRepository.GetAllBookings();

        if (!result.Any())
        {
            throw new NotFoundException(nameof(IEnumerable<BookingDTO>));
        }
        
        return _mapper.Map<IEnumerable<BookingDTO>>(result);
    }
}