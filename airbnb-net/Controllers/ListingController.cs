using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Querying.Queries.ListingQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : InternalControllerBase
    {
        private readonly IUserIdGetter _userIdGetter;
        private readonly IMediator _mediator;

        public ListingController(ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator) : base(logger,mapper)
        {
            _userIdGetter = userIdGetter;
            _mediator = mediator;
        }
        
        // GET: api/Listing
        [HttpGet]
        public async Task<IEnumerable<ListingDTO>> Get()
        {
            return await _mediator.Send(new GetAllListingsQuery());
        }
        
        // GET: api/Listing/5
        [HttpGet("{id:guid}")]
        public async Task<ListingDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetListingByIdQuery(id));
        }
        
        // POST: api/Listing
        [HttpPost]
        public async Task<Guid> Post([FromBody] ListingCreateDTO listingCreate)
        {
            return await _mediator.Send(new CreateListingCommand(listingCreate));
        }
        
        // PUT: api/Listing/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] ListingUpdateDTO listingUpdate)
        {
            await _mediator.Send(new UpdateListingCommand(id, listingUpdate));
        }
        
        // DELETE: api/Listing/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteListingCommand(id));
        }
    }
}
