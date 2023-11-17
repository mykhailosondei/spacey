using ApplicationCommon.DTOs.Booking;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class UpdateBookingCommandHandler : BaseHandler, IRequestHandler<UpdateBookingCommand>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    
    public UpdateBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
    }

    public async Task Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingDTO = _mapper.Map<BookingDTO>(request.Booking);
        var booking = _mapper.Map<Booking>(bookingDTO);
            
        await _bookingCommandAccess.UpdateBooking(request.Id ,booking);
    }
}