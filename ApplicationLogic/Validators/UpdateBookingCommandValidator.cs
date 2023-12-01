using ApplicationLogic.Commanding.Commands.BookingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(request => request.Booking.CheckIn).NotEmpty();
        RuleFor(request => request.Booking.CheckOut).NotEmpty();
        RuleFor(request => request.Booking.NumberOfGuests).NotEmpty().GreaterThan(0);
    }
}