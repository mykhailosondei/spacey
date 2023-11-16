using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.User;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationLogic.Commanding.Commands.UserCommands;
using ApplicationLogic.Querying.Queries.UserQueries;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : InternalControllerBase
    {
        private readonly IMediator _mediator;
        
        public UserController(ILogger<InternalControllerBase> logger, IMapper mapper, IMediator mediator)
        : base(logger, mapper)
        {
            _mediator = mediator;
        }
        
        // GET: api/User
        [HttpGet]
        public async Task<IEnumerable<UserDTO>> Get()
        {
            return await _mediator.Send(new GetAllUsersQuery());
        }

        // GET: api/User/5
        [HttpGet("{id:guid}")]
        public async Task<UserDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetUserByIdQuery(id));
        }

        // PUT: api/User/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] UserUpdateDTO userUpdate)
        {
            await _mediator.Send(new UpdateUserCommand(id, userUpdate));
        }

        // DELETE: api/User/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteUserCommand(id));
        }
    }
}
