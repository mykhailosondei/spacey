using ApplicationCommon.DTOs.BookingDTOs;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record CreateBookingCommand(BookingCreateDTO Booking) : IRequest<Guid>, ICommand;