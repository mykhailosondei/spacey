using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(r => r.Listing.HostId).NotEmpty().Must((guid)=> guid != Guid.Empty);
        RuleFor(r => r.Listing.Title).NotEmpty().MaximumLength(50);
        RuleFor(r => r.Listing.Description).NotEmpty().MaximumLength(500);
        RuleFor(r => r.Listing.PropertyType).NotEmpty();
        RuleFor(r => r.Listing.PropertyType).Must((propertyType) => Enum.IsDefined(typeof(PropertyType), propertyType));
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