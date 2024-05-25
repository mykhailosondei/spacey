
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.BookingQueries;
using AutoMapper;
using CustomMediator;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace ApplicationLogic.Querying.QueryHandlers.BookingHandlers;

public class GetBookingByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingByIdQuery, BookingDTO>
{
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IBookingCommandAccess _bookingCommandAccess;
    
    public GetBookingByIdQueryHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository, IBookingCommandAccess bookingCommandAccess) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
        _bookingCommandAccess = bookingCommandAccess;
    }

    public async Task<BookingDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookingQueryRepository.GetBookingById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(BookingDTO));
        }
        
        result.LastAccess = DateTime.UtcNow;
        
        var mappedBooking = _mapper.Map<BookingDTO>(result);
        
        return mappedBooking;
    }
}