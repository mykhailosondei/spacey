using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(r => r.Listing.HostId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(r => r.Listing.Title).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Description).NotEmpty().MaximumLength(1500);
        RuleFor(r => r.Listing.PropertyType).NotNull();
        RuleFor(r => r.Listing.PropertyType).Must((propertyType) => Enum.IsDefined(typeof(PropertyType), propertyType));
        RuleFor(r => r.Listing.PricePerNight).NotEmpty().GreaterThanOrEqualTo(15).LessThanOrEqualTo(10000);
        RuleFor(r => r.Listing.NumberOfRooms).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.NumberOfBathrooms).NotNull().GreaterThanOrEqualTo(0);
        RuleFor(r => r.Listing.NumberOfGuests).NotEmpty().GreaterThan(0);
        RuleFor(r => r.Listing.Address).NotNull();
        RuleFor(r => r.Listing.Address).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Amenities).NotNull();
    }
}