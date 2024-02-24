using ApplicationLogic.Commanding.Commands.BookingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(request => request.Booking.ListingId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(request => request.Booking.UserId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(request => request.Booking.CheckIn).NotEmpty().GreaterThan(DateTime.Today);
        RuleFor(request => request.Booking.CheckOut).NotEmpty().GreaterThan(request => request.Booking.CheckIn);
        RuleFor(request => request.Booking.NumberOfGuests).NotEmpty().GreaterThan(0);
    }
}