
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
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
        
        if (result == null)
        {
            throw new NotFoundException(nameof(BookingDTO));
        }
        
        return _mapper.Map<BookingDTO>(result);
    }
}