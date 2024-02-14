using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.BookingDTOs;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.CommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Commanding.Commands.BookingCommands;
using ApplicationLogic.Querying.Queries.BookingQueries;
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
    public class BookingController : InternalControllerBase
    {
        private readonly IUserIdGetter _userIdGetter;
        private readonly IMediator _mediator;

        public BookingController (ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator) : base(logger,mapper)
        {
            _userIdGetter = userIdGetter;
            _mediator = mediator;
        }
        
        // GET: api/Booking
        [HttpGet]
        public async Task<IEnumerable<BookingDTO>> Get()
        {
            return await _mediator.Send(new GetAllBookingsQuery());
        }
        
        // GET: api/Booking/5
        [HttpGet("{id:guid}")]
        public async Task<BookingDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetBookingByIdQuery(id));
        }
        
        // POST: api/Booking
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<Guid> Post([FromBody] BookingCreateDTO bookingCreate)
        {
            bookingCreate.UserId = _userIdGetter.UserId;
            return await _mediator.Send(new CreateBookingCommand(bookingCreate));
        }
        
        // PUT: api/Booking/5
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task Put(Guid id, [FromBody] BookingUpdateDTO bookingUpdate)
        {
            await _mediator.Send(new UpdateBookingCommand(id, bookingUpdate));
        }
        
        // DELETE: api/Booking/5
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "User")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteBookingCommand(id));
        }
        
        // GET: api/Booking/5/cancel
        [HttpPost("{id:guid}/cancel")]
        [Authorize(Roles = "User")]
        public async Task Cancel(Guid id)
        {
            await _mediator.Send(new CancelBookingCommand(id));
        }
    }
}
