using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record CancelBookingCommand(Guid Id) : IRequest, ICommand;