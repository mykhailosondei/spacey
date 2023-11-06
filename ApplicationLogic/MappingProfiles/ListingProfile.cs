using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationDAL.Entities;
using ApplicationLogic.Utilities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class ListingProfile : Profile
{
    public ListingProfile()
    {
        CreateMap<ListingCreateDTO, Listing>()
            .ForMember(dest => dest.Amenities,
                opt => opt.MapFrom(src => MapperUtilities.ConstructAmenitiesFromStringArray(src.Amenities)))
            .ForMember(dest => dest.Host,
                opt => opt.MapFrom(src => new Host { Id = src.HostId }));
        CreateMap<ListingDTO, Listing>().ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructAmenitiesFromStringArray(src.Amenities)));
        CreateMap<Listing, ListingDTO>().ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructStringArrayFromAmenities(src.Amenities)));
    }
}