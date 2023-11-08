using ApplicationCommon.DTOs.Host;
using ApplicationCommon.DTOs.Image;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.Entities;
using AutoMapper;

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
        CreateMap<UserUpdateDTO, UserDTO>();
    }
}