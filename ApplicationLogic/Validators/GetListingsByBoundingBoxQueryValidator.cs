using ApplicationLogic.Querying.Queries.ListingQueries;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class GetListingsByBoundingBoxQueryValidator : AbstractValidator<GetListingsByBoundingBoxQuery>
{
    public GetListingsByBoundingBoxQueryValidator()
    {
        RuleFor(x => x.BoundingBox).NotEmpty().WithMessage("Bounding box cannot be empty");
        RuleFor(x => x.BoundingBox).Must(x => x.IsValid()).WithMessage("Bounding box must be valid");
    }
}