using ApplicationCommon.DTOs.Booking;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record CreateBookingCommand(BookingCreateDTO Booking) : IRequest<Guid>;