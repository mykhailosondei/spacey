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
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.ListingQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : InternalControllerBase
    {
        private readonly IHostIdGetter _hostIdGetter;
        private readonly IMediator _mediator;

        public ListingController(ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator, IHostIdGetter hostIdGetter) : base(logger,mapper)
        {
            _mediator = mediator;
            _hostIdGetter = hostIdGetter;
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
        
        [HttpGet("{propertyType}")]
        public async Task<IEnumerable<ListingDTO>> Get(string propertyType)
        {
            return await _mediator.Send(new GetListingsByPropertyTypeQuery(propertyType));
        }
        
        // POST: api/Listing
        [HttpPost]
        [Authorize(Roles = "Host")]
        public async Task<Guid> Post([FromBody] ListingCreateDTO listingCreate)
        {
            listingCreate.HostId = _hostIdGetter.HostId;
            return await _mediator.Send(new CreateListingCommand(listingCreate));
        }
        
        // PUT: api/Listing/5
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Host")]
        public async Task Put(Guid id, [FromBody] ListingUpdateDTO listingUpdate)
        {
            await _mediator.Send(new UpdateListingCommand(id, listingUpdate));
        }
        
        // DELETE: api/Listing/5
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Host")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteListingCommand(id));
        }
        
        // POST: api/Listing/123/like
        [HttpPost("{id:guid}/like")]
        [Authorize(Roles = "User")]
        public async Task Like(Guid id)
        {
            await _mediator.Send(new LikeListingCommand(id));
        }
        
        // POST: api/Listing/123/unlike
        [HttpPost("{id:guid}/unlike")]
        [Authorize(Roles = "User")]
        public async Task Unlike(Guid id)
        {
            await _mediator.Send(new UnlikeListingCommand(id));
        }
    }
}
