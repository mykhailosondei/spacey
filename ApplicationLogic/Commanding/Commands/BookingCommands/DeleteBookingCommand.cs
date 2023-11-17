using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record DeleteBookingCommand(Guid Id) : IRequest;