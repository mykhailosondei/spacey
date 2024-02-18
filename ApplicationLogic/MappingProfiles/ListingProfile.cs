using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.Enums;
using ApplicationDAL.Entities;
using ApplicationCommon.Utilities;
using AutoMapper;
using MongoDB.Driver.GeoJsonObjectModel;

namespace ApplicationLogic.MappingProfiles;

public class ListingProfile : Profile
{
    public ListingProfile()
    {
        CreateMap<ListingCreateDTO, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Host,
                opt => opt.MapFrom(src => new HostDTO
                {
                    Id = src.HostId,
                    ListingsIds = new List<Guid>()
                })).ForMember(dest => dest.Address, opt => opt.MapFrom(
                (src, dest, member, context) => UserProfile.AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result));;
            #endregion
        CreateMap<ListingUpdateDTO, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Host,
                opt => opt.MapFrom(src => new HostDTO { Id = src.HostId }));
            #endregion
        CreateMap<ListingDTO, Listing>()
            #region Mapping
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructAmenitiesFromStringArray(src.Amenities)))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(src.Longitude, src.Latitude))))
            .AfterMap((src, dest, context) =>
            {
                dest.Host = context.Mapper.Map<HostDTO, Host>(src.Host);
            });
            #endregion
        CreateMap<Listing, ListingDTO>()
            #region Mapping
            .ForMember(dest => dest.Amenities, opt => opt.MapFrom(src => MapperUtilities.ConstructStringArrayFromAmenities(src.Amenities)))
            .ForMember(dest => dest.Latitude, opt => opt.MapFrom(src => src.Location.Coordinates.Y))
            .ForMember(dest => dest.Longitude, opt => opt.MapFrom(src => src.Location.Coordinates.X))
            .AfterMap((src, dest, context) =>
            {
                dest.Host = context.Mapper.Map<Host, HostDTO>(src.Host);
            });
            #endregion
    }
}