using ApplicationCommon.Enums;
using ApplicationLogic.Querying.Queries.ListingQueries;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class GetListingsByPropertyTypeQueryValidator : AbstractValidator<GetListingsByPropertyTypeQuery>
{
    public GetListingsByPropertyTypeQueryValidator()
    {
        RuleFor(x => x.PropertyType).NotEmpty().WithMessage("Property type cannot be empty");
        RuleFor(x => x.PropertyType).Must(x => Enum.IsDefined(typeof(PropertyType), x))
            .WithMessage("Property type must be a valid property type");
    }
}