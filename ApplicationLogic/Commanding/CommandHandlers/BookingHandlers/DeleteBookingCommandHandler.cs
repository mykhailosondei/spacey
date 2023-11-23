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
    
    public DeleteBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess, IBookingQueryRepository bookingQueryRepository, IUserIdGetter userIdGetter) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
        _bookingQueryRepository = bookingQueryRepository;
        _userIdGetter = userIdGetter;
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
        
        await _bookingCommandAccess.DeleteBooking(request.Id);
    }
}