using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.BookingHandlers;

public class DeleteBookingCommandHandler : BaseHandler, IRequestHandler<DeleteBookingCommand>
{
    private readonly IBookingCommandAccess _bookingCommandAccess;
    
    public DeleteBookingCommandHandler(IMapper mapper, IBookingCommandAccess bookingCommandAccess) : base(mapper)
    {
        _bookingCommandAccess = bookingCommandAccess;
    }

    public async Task Handle(DeleteBookingCommand request, CancellationToken cancellationToken)
    {
        await _bookingCommandAccess.DeleteBooking(request.Id);
    }
}