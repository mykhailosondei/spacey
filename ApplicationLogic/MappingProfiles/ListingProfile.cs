using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationDAL.Entities;
using ApplicationCommon.Utilities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class ListingProfile : Profile
{
    public ListingProfile()
    {
        CreateMap<ListingCreateDTO, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Host,
                opt => opt.MapFrom(src => new HostDTO { Id = src.HostId }));
            #endregion
        CreateMap<ListingUpdateDTO, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Host,
                opt => opt.MapFrom(src => new HostDTO { Id = src.HostId }));
            #endregion
        CreateMap<ListingDTO, Listing>()
            #region Mapping
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructAmenitiesFromStringArray(src.Amenities)))
            .AfterMap((src, dest, context) =>
            {
                dest.Host = context.Mapper.Map<HostDTO, Host>(src.Host);
            });
            #endregion
        CreateMap<Listing, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructStringArrayFromAmenities(src.Amenities)))
            .AfterMap((src, dest, context) =>
            {
                dest.Host = context.Mapper.Map<Host, HostDTO>(src.Host);
            });
            #endregion
    }
}