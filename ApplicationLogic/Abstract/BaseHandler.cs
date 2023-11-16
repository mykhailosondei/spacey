using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApplicationLogic.Abstract;

public abstract class BaseHandler
{
    protected readonly IMapper _mapper;

    protected BaseHandler(IMapper mapper)
    {
        _mapper = mapper;
    }
}