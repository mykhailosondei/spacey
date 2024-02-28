using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ConversationHandlers;

public class GetConversationByBookingIdQueryHandler : BaseHandler, IRequestHandler<GetConversationByBookingIdQuery, Conversation>
{
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    
    public GetConversationByBookingIdQueryHandler(IMapper mapper, IConversationQueryRepository conversationQueryRepository, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IHostIdGetter hostIdGetter, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _conversationQueryRepository = conversationQueryRepository;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _hostIdGetter = hostIdGetter;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<Conversation> Handle(GetConversationByBookingIdQuery request, CancellationToken cancellationToken)
    {
        var booking = await _bookingQueryRepository.GetBookingById(request.BookingId);
        
        if (booking is null)
        {
            throw new NotFoundException("Booking");
        }
        
        var listing = await _listingQueryRepository.GetListingById(booking.ListingId);
        
        if (listing is null)
        {
            throw new NotFoundException("Listing");
        }
        
        if (booking.UserId != _userIdGetter.UserId && listing.Host.Id != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }
        
        return await _conversationQueryRepository.GetConversationByBookingId(request.BookingId);
    }
}