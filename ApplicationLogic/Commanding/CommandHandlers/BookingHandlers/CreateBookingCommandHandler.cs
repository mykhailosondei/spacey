using ApplicationCommon.DTOs.Booking;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class CreateBookingCommandHandler : BaseHandler, IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    
    public CreateBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
    }

    public async Task<Guid> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        var bookingDTO = _mapper.Map<BookingDTO>(request.Booking);
        var booking = _mapper.Map<Booking>(bookingDTO);
        
        return await _bookingCommandAccess.AddBooking(booking);
    }
}