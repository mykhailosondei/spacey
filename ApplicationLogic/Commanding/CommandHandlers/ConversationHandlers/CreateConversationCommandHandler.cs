using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ConversationCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ConversationHandlers;

public class CreateConversationCommandHandler : BaseHandler, IRequestHandler<CreateConversationCommand, Guid>
{
    private readonly IConversationCommandAccess _conversationCommandAccess;
    private readonly IConversationQueryRepository _conversationQueryRepository;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IHostIdGetter _hostIdGetter;
    
    public CreateConversationCommandHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository, IUserQueryRepository userQueryRepository, IHostQueryRepository hostQueryRepository, IConversationCommandAccess conversationCommandAccess, IConversationQueryRepository conversationQueryRepository, IHostIdGetter hostIdGetter, IUserIdGetter userIdGetter, IListingQueryRepository listingQueryRepository) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
        _userQueryRepository = userQueryRepository;
        _hostQueryRepository = hostQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
        _conversationQueryRepository = conversationQueryRepository;
        _hostIdGetter = hostIdGetter;
        _userIdGetter = userIdGetter;
        _listingQueryRepository = listingQueryRepository;
    }

    public async Task<Guid> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
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
        
        var conversation = await _conversationQueryRepository.GetConversationByBookingId(request.BookingId);
        
        if (conversation is not null)
        {
            throw new InvalidOperationException("Conversation already exists");
        }
        
        return await _conversationCommandAccess.CreateConversation(userId: booking.UserId, hostId: listing.Host.Id, bookingId: booking.Id);
    }
}