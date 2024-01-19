using ApplicationCommon.DTOs.Image;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageDTO>();
        CreateMap<ImageDTO, Image>();
    }
}