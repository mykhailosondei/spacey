using ApplicationCommon.DTOs.Host;
using ApplicationDAL.Entities;
using AutoMapper;

namespace ApplicationLogic.MappingProfiles;

public class HostProfile : Profile
{
    public HostProfile()
    {
        CreateMap<HostDTO, Host>();
        CreateMap<Host, HostDTO>();
    }
}