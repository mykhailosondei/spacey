
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.Querying.Queries.BookingQueries;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace ApplicationLogic.Querying.QueryHandlers.BookingHandlers;

public class GetBookingByIdQueryHandler : BaseHandler, IRequestHandler<GetBookingByIdQuery, BookingDTO>
{
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IBookingCommandAccess _bookingCommandAccess;
    private readonly IDistributedCache _distributedCache;
    
    public GetBookingByIdQueryHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository, IDistributedCache distributedCache, IBookingCommandAccess bookingCommandAccess) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
        _distributedCache = distributedCache;
        _bookingCommandAccess = bookingCommandAccess;
    }

    public async Task<BookingDTO> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        string cacheKey = $"booking-{request.Id}";
        string cacheKeyTimestamp = $"booking-{request.Id}-timestamp";
        
        var cachedBooking = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);
        
        if (cachedBooking != null)
        {
            BookingDTO cachedBookingDTO = BsonSerializer.Deserialize<BookingDTO>(cachedBooking);
            cachedBookingDTO.LastAccess = DateTime.UtcNow;
            await _distributedCache.SetStringAsync($"booking-{request.Id}-timestamp", JsonConvert.SerializeObject(cachedBookingDTO.LastAccess), cancellationToken);
            return cachedBookingDTO;
        }
        
        var result = await _bookingQueryRepository.GetBookingById(request.Id);
        
        if (result == null)
        {
            throw new NotFoundException(nameof(BookingDTO));
        }
        
        result.LastAccess = DateTime.UtcNow;
        
        await _distributedCache.SetStringAsync(cacheKey, result.ToBsonDocument().ToString(), cancellationToken);
        await _distributedCache.SetStringAsync(cacheKeyTimestamp, JsonConvert.SerializeObject(result.LastAccess), cancellationToken);
        
        return _mapper.Map<BookingDTO>(result);
    }
}