using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Filters;
using ApplicationLogic.Querying.Queries.ListingQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ListingHandlers;

public class ListingAndBookings
{
    public ListingDTO Listing { get; set; }
    public IEnumerable<BookingDTO> Bookings { get; set; }

    public ListingAndBookings()
    {
        Bookings = new List<BookingDTO>();
    }
}

public class GetListingsBySearchQueryHandler : BaseHandler, IRequestHandler<GetListingsBySearchQuery, IEnumerable<ListingDTO>>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IBookingQueryRepository _bookingQueryRepository;

    public GetListingsBySearchQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository, IBookingQueryRepository bookingQueryRepository) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task<IEnumerable<ListingDTO>> Handle(GetListingsBySearchQuery request, CancellationToken cancellationToken)
    {
        var listingAndBookings = new List<ListingAndBookings>();
        
        var query = await _listingQueryRepository.GetAllListings();
        
        var listings = _mapper.Map<IEnumerable<ListingDTO>>(query).ToList();
        
        foreach (ListingDTO listing in listings)
        {
            List<BookingDTO> bookings = new List<BookingDTO>();
            foreach (var bookingId in listing.BookingsIds)
            {
                var booking = await _bookingQueryRepository.GetBookingById(bookingId);
                if (booking == null) continue;
                bookings.Add(_mapper.Map<BookingDTO>(booking));
            }
            listingAndBookings.Add(new ListingAndBookings
            {
                Listing = listing,
                Bookings = bookings
            });
        }
        
        foreach (var filter in request.Filters)
        {
            listingAndBookings = await filter.ApplyFilter(listingAndBookings);
        }
        
        listings = listingAndBookings.Select(listingAndBookings => listingAndBookings.Listing).Skip((int)request.From).Take((int)(request.To - request.From)).ToList();
        
        return listings;
    }
}