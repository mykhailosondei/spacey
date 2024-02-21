using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;
using AutoMapper.Execution;
using BingMapsRESTToolkit;
using Address = ApplicationCommon.Structs.Address;

namespace ApplicationLogic.MappingProfiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<RegisterUserDTO, UserDTO>();
        CreateMap<User, UserDTO>()
            .AfterMap((src, dest, context) =>
            {
                dest.Avatar = context.Mapper.Map<Image, ImageDTO>(src.Avatar);
                dest.Host = context.Mapper.Map<Host, HostDTO>(src.Host);
            })
            .ReverseMap()
            .AfterMap((src, dest, context) =>
            {
                dest.Avatar = context.Mapper.Map<ImageDTO, Image>(src.Avatar);
                dest.Host = context.Mapper.Map<HostDTO, Host>(src.Host);
            });
        CreateMap<UserUpdateDTO, UserDTO>().BeforeMap((src, dest, context) =>
        {
            dest.Address = AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result;
        });
        CreateMap<UserUpdateDTO, UserDTO>().ForMember(dest => dest.Address, opt => opt.MapFrom(
            (src, dest, member, context) => AddressFromString(src.Address, context.Items["BingMapsKey"].ToString()!).Result));
    }

    public static async Task<Address> AddressFromString(string address, string key)
    {
        if (string.IsNullOrEmpty(address))
        {
            return new Address();
        }
        
        var request = new GeocodeRequest()
        {
            Query = address,
            BingMapsKey = key
        };
        
        var response = await request.Execute();

        if (response.StatusCode != 200)
        {
            throw new Exception("Error while geocoding address.");
        }

        var result = response.ResourceSets[0].Resources[0] as Location;
        
        if (result == null)
        {
            throw new Exception("Error while geocoding address.");
        }
        
        var locationRequest = new ReverseGeocodeRequest()
        {
            Point = result.Point.GetCoordinate(),
            BingMapsKey = key
        };
        
        var locationResponse = await locationRequest.Execute();
        
        if (locationResponse.StatusCode != 200)
        {
            throw new Exception("Error while geocoding address.");
        }
        
        var locationResult = locationResponse.ResourceSets[0].Resources[0] as Location;

        if (locationResult == null)
        {
            throw new Exception("Error while geocoding address.");
        }
        
        var addressStruct = new Address()
        {
            Street = locationResult.Address.AddressLine,
            City = locationResult.Address.Locality,
            Country = locationResult.Address.CountryRegion
        };

        if (addressStruct.Street == null || addressStruct.City == null || addressStruct.Country == null)
        {
            throw new Exception("Error while geocoding address.");
        }
        
        return addressStruct;
    }

}