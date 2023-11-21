using ApplicationCommon.DTOs.Listing;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(r => r.Listing.Title).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Description).NotEmpty().MaximumLength(500);
        RuleFor(r => r.Listing.PropertyType).NotEmpty();
        RuleFor(r => r.Listing.PricePerNight).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.NumberOfRooms).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.NumberOfBathrooms).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.NumberOfGuests).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.Address).NotNull();
        RuleFor(r => r.Listing.Address.Country).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Address.City).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Address.Street).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Amenities).NotNull();
    }
}