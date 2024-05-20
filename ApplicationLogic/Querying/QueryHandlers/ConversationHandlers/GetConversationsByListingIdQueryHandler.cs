using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Querying.QueryHandlers.ConversationHandlers;

public class GetConversationsByListingIdQueryHandler : BaseHandler, IRequestHandler<GetConversationsByListingIdQuery, IEnumerable<Conversation>>
{

    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    
    public GetConversationsByListingIdQueryHandler(IMapper mapper, IListingQueryRepository listingQueryRepository, IConversationQueryRepository conversationQueryRepository, IHostIdGetter hostIdGetter) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
        _conversationQueryRepository = conversationQueryRepository;
        _hostIdGetter = hostIdGetter;
    }

    public async Task<IEnumerable<Conversation>> Handle(GetConversationsByListingIdQuery request, CancellationToken cancellationToken)
    { 
        var listing = await _listingQueryRepository.GetListingById(request.ListingId);
        if (listing == null)
        {
            throw new NotFoundException("Listing not found");
        }
        var hostId = _hostIdGetter.HostId;
        if (listing.Host.Id != hostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to view this listing's conversations");
        }

        return await Task.WhenAll(listing.BookingsIds.Select(async bookingId =>
            await _conversationQueryRepository.GetConversationByBookingId(bookingId)));
    }
}