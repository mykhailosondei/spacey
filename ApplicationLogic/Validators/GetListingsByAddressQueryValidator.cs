using ApplicationCommon.Structs;
using ApplicationLogic.Querying.Queries.ListingQueries;
using FluentValidation;

namespace ApplicationLogic.Validators;

public class GetListingsByAddressQueryValidator : AbstractValidator<GetListingsByAddressQuery>
{
    public GetListingsByAddressQueryValidator()
    {
        RuleFor(query => query.Address).NotNull();
        RuleFor(query => query).Must(query => IsValid(query.Address));
    }

    private static bool IsValid(Address address)
    {
        var cityStatus = address.City != null;
        var countryStatus = address.Country != null;
        var streetStatus = address.Street != null;
        return ( streetStatus &&  cityStatus && countryStatus) ||
               (!streetStatus &&  cityStatus && countryStatus) ||
               (!streetStatus && !cityStatus && countryStatus);
    }
}