using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.BookingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.BookingHandlers;

public class GetBookingsByStatusQueryHandler : BaseHandler ,IRequestHandler<GetBookingsByStatusQuery, IEnumerable<BookingDTO>>
{
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetBookingsByStatusQueryHandler(IMapper mapper, IHostIdGetter hostIdGetter, IHostQueryRepository hostQueryRepository, IBookingQueryRepository bookingQueryRepository, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _hostIdGetter = hostIdGetter;
        _hostQueryRepository = hostQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<IEnumerable<BookingDTO>> Handle(GetBookingsByStatusQuery request, CancellationToken cancellationToken)
    {
        var host = await _hostQueryRepository.GetHostById(_hostIdGetter.HostId);
        if (host == null) throw new UnauthorizedAccessException("You are not authorized to view this page.");

        var listings = await _listingQueryRepository.GetListingsByHostId(host.Id);

        var bookings = new List<Booking>();
        
        foreach (var listing in listings)
        {
            var listingBookings = await _bookingQueryRepository.GetBookingsByListingId(listing.Id);
            bookings.AddRange(listingBookings.Where(b => b.Status == request.Status));
        }
        
        return _mapper.Map<IEnumerable<BookingDTO>>(bookings);
    }
}