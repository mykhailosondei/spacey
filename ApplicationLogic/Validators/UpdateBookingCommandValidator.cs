using ApplicationLogic.Commanding.Commands.BookingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(request => request.Booking.CheckIn).NotEmpty().GreaterThan(DateTime.Today);
        RuleFor(request => request.Booking.CheckOut).NotEmpty().GreaterThan(request => request.Booking.CheckIn);
        RuleFor(request => request.Booking.NumberOfGuests).NotEmpty().GreaterThan(0);
    }
}