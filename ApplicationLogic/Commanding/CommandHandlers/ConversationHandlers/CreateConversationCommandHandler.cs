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
    private readonly IUserIdGetter _userIdGetter;
    private readonly IHostIdGetter _hostIdGetter;
    
    public CreateConversationCommandHandler(IMapper mapper, IBookingQueryRepository bookingQueryRepository, IUserQueryRepository userQueryRepository, IHostQueryRepository hostQueryRepository, IConversationCommandAccess conversationCommandAccess, IConversationQueryRepository conversationQueryRepository, IHostIdGetter hostIdGetter, IUserIdGetter userIdGetter) : base(mapper)
    {
        _bookingQueryRepository = bookingQueryRepository;
        _userQueryRepository = userQueryRepository;
        _hostQueryRepository = hostQueryRepository;
        _conversationCommandAccess = conversationCommandAccess;
        _conversationQueryRepository = conversationQueryRepository;
        _hostIdGetter = hostIdGetter;
        _userIdGetter = userIdGetter;
    }

    public async Task<Guid> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        if (request.UserId != _userIdGetter.UserId && request.HostId != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to perform this action");
        }
        
        var conversation = await _conversationQueryRepository.GetConversationByBookingId(request.BookingId);
        
        if (conversation is not null)
        {
            throw new InvalidOperationException("Conversation already exists");
        }
        
        var booking = await _bookingQueryRepository.GetBookingById(request.BookingId);
        var user = await _userQueryRepository.GetUserById(request.UserId);
        var host = await _hostQueryRepository.GetHostById(request.HostId);

        if (booking is null)
        {
            throw new NotFoundException("Booking");
        }
        
        if (user is null)
        {
            throw new NotFoundException("User");
        }
        
        if (host is null)
        {
            throw new NotFoundException("Host");
        }
        
        if (host.ListingsIds.TrueForAll(id => id != booking.ListingId))
        {
            throw new UnauthorizedAccessException("Host does not own this listing");
        }

        if (user.Host.Id == host.Id || user.Id == host.UserId)
        {
            throw new UnauthorizedAccessException("Cannot create conversation with yourself");
        }

        if (user.BookingsIds.TrueForAll(id => id != booking.Id))
        {
            throw new UnauthorizedAccessException("User does not own this booking");
        }
        
        return await _conversationCommandAccess.CreateConversation(userId: user.Id, hostId: host.Id, bookingId: booking.Id);
    }
}