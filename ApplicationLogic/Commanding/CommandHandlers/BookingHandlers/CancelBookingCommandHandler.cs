using ApplicationCommon.Enums;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using CustomMediator;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class CancelBookingCommandHandler : BaseHandler, IRequestHandler<CancelBookingCommand>
{
    
    private readonly IUserIdGetter _userIdGetter;
    private readonly IBookingQueryRepository _bookingQueryRepository;
    private readonly IBookingCommandAccess _bookingCommandAccess;

    public CancelBookingCommandHandler(IMapper mapper, IUserIdGetter userIdGetter, IBookingCommandAccess bookingCommandAccess, IBookingQueryRepository bookingQueryRepository) : base(mapper)
    {
        _userIdGetter = userIdGetter;
        _bookingCommandAccess = bookingCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
    }

    public async Task Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingQueryRepository.GetBookingById(request.Id);
        
        if (booking == null)
        {
            throw new NotFoundException("Booking");
        }
        
        if (booking.UserId != _userIdGetter.UserId)
        {
            throw new UnauthorizedAccessException();
        }
        
        if (booking.Status == BookingStatus.Cancelled)
        {
            throw new InvalidOperationException("Booking is already canceled.");
        }
        
        booking.Status = BookingStatus.Cancelled;
        
        await _bookingCommandAccess.UpdateBooking(booking.Id, booking);
    }
}