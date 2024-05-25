using ApplicationCommon.DTOs.BookingDTOs;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record CreateBookingCommand(BookingCreateDTO Booking) : IRequest<Guid>, ICommand;