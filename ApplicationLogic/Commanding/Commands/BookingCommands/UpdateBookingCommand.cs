using ApplicationCommon.DTOs.BookingDTOs;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record UpdateBookingCommand(Guid Id, BookingUpdateDTO Booking) : IRequest, ICommand;