using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers.Abstract;

public class InternalControllerBase : ControllerBase
{
    protected readonly ILogger<InternalControllerBase> _logger;
    protected readonly IMapper _mapper;
    public InternalControllerBase(ILogger<InternalControllerBase> logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
    }
}