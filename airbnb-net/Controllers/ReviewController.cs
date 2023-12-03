using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Review;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.ReviewCommands;
using ApplicationLogic.Querying.Queries.ReviewQueries;
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
    public class ReviewController : InternalControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserIdGetter _userIdGetter;
        
        public ReviewController(ILogger<InternalControllerBase> logger, IMapper mapper, IMediator mediator, IUserIdGetter userIdGetter)
        : base(logger, mapper)
        {
            _mediator = mediator;
            _userIdGetter = userIdGetter;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<IEnumerable<ReviewDTO>> Get()
        {
            return await _mediator.Send(new GetAllReviewsQuery());
        }

        // GET: api/Review/5
        [HttpGet("{id:guid}", Name = "Get")]
        public async Task<ReviewDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetReviewByIdQuery(id));
        }

        // POST: api/Review
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<Guid> Post([FromBody] ReviewCreateDTO reviewCreate)
        { 
            reviewCreate.UserId = _userIdGetter.UserId;
            return await _mediator.Send(new CreateReviewCommand(reviewCreate));
        }

        // PUT: api/Review/5
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task Put(Guid id, [FromBody] ReviewUpdateDTO reviewUpdate)
        {
            await _mediator.Send(new UpdateReviewCommand(id, reviewUpdate));
        }

        // DELETE: api/Review/5
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteReviewCommand(id));
        }
    }
}
