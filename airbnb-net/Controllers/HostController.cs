using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Host;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.HostCommands;
using ApplicationLogic.Querying.Queries.HostQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Host = ApplicationDAL.Entities.Host;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HostController : InternalControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHostCommandAccess _hostCommandAccess;
        private readonly IUserIdGetter _userIdGetter;


        public HostController(IHostQueryRepository hostQueryRepository, IHostCommandAccess hostCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator) 
            : base(logger, mapper)
        {
            _hostCommandAccess = hostCommandAccess;
            _userIdGetter = userIdGetter;
            _mediator = mediator;
        }
        
        // GET: api/Host/5
        [HttpGet("{id:guid}")]
        public async Task<HostDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetHostByIdQuery(id));
        }
        
        // POST: api/Host
        [HttpPost]
        public async Task<Guid> Post([FromBody] HostCreateDTO hostCreate)
        {
            hostCreate.UserId = _userIdGetter.UserId;
            return await _mediator.Send(new CreateHostCommand(hostCreate));
        }
        
        // PUT: api/Host/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] HostUpdateDTO hostUpdate)
        {
            hostUpdate.UserId = _userIdGetter.UserId;
            await _mediator.Send(new UpdateHostCommand(id, hostUpdate));
        }
        
        // DELETE: api/Host/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _hostCommandAccess.DeleteHost(id);
        }
    }
}
