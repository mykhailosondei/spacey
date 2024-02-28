using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using ApplicationLogic.Commanding.Commands.ConversationCommands;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User, Host")]
    public class ConversationController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public ConversationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<Guid> CreateConversation(Guid userId, Guid hostId, Guid bookingId)
        {
            return await _mediator.Send(new CreateConversationCommand(userId, hostId, bookingId));
        }
        
        [HttpGet("/request")]
        public async Task<IEnumerable<Conversation>> GetConversations(ConversationsRequest request)
        {
            return await _mediator.Send(new GetConversationsQuery(request));
        }
        
        [HttpGet("{conversationId:guid}")]
        public async Task<Conversation> GetConversation(Guid conversationId)
        {
            return await _mediator.Send(new GetConversationByIdQuery(conversationId));
        }
        
        [HttpGet("/booking/{bookingId:guid}")]
        public async Task<Conversation> GetConversationByBookingId(Guid bookingId)
        {
            return await _mediator.Send(new GetConversationByBookingIdQuery(bookingId));
        }
        
        [HttpDelete("{conversationId:guid}")]
        public async Task DeleteConversation(Guid conversationId)
        {
            await _mediator.Send(new DeleteConversationCommand(conversationId));
        }
    }
}
