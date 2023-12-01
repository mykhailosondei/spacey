using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class DeleteBookingCommandHandler : BaseHandler, IRequestHandler<DeleteBookingCommand>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IPublisher _publisher;
    
    public DeleteBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter, IPublisher publisher) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
        _publisher = publisher;
    }

    public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingQueryRepository.GetBookingById(request.Id);
        
        if (booking == null)
        {
            throw new NotFoundException("Booking");
        }
        
        if(booking.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this booking.");
        }
        
        await _publisher.Publish(new BookingDeletedEvent()
        {
            BookingId = booking.Id,
            ListingId = booking.ListingId,
            UserId = booking.UserId,
            DeletedAt = DateTime.UtcNow
        }, CancellationToken.None);
        
        await _bookingCommandAccess.DeleteBooking(request.Id);
    }
}