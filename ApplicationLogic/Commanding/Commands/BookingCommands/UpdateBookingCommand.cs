using ApplicationCommon.DTOs.BookingDTOs;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.BookingCommands;

public record UpdateBookingCommand(Guid Id, BookingUpdateDTO Booking) : IRequest, ICommand;