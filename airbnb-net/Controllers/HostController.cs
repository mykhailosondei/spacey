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
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.HostQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IHostIdGetter _hostIdGetter;


        public HostController(IHostQueryRepository hostQueryRepository, IHostCommandAccess hostCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator, IHostIdGetter hostIdGetter) 
            : base(logger, mapper)
        {
            _hostCommandAccess = hostCommandAccess;
            _userIdGetter = userIdGetter;
            _mediator = mediator;
            _hostIdGetter = hostIdGetter;
        }
        
        // GET: api/Host/5
        [HttpGet("{id:guid}")]
        public async Task<HostDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetHostByIdQuery(id));
        }
        
        // PUT: api/Host/5
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Host, User")]
        public async Task Put(Guid id, [FromBody] HostUpdateDTO hostUpdate)
        {
            hostUpdate.UserId = _userIdGetter.UserId;
            hostUpdate.Id = _hostIdGetter.HostId;
            await _mediator.Send(new UpdateHostCommand(id, hostUpdate));
        }
        
        // GET: api/Host
        [HttpGet("fromToken")]
        [Authorize(Roles = "Host")]
        public async Task<HostDTO> Get()
        {
            return await _mediator.Send(new GetHostByIdQuery(_hostIdGetter.HostId));
        }
        
        
        // DELETE: api/Host/5
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Host")]
        public async Task Delete(Guid id)
        {
            await _hostCommandAccess.DeleteHost(id);
        }
    }
}
