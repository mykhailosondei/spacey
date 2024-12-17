using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCommon.Structs;
using ApplicationDAL.Entities;
using ApplicationLogic.Commanding.CommandHandlers.ConversationHandlers;
using ApplicationLogic.Commanding.Commands.ConversationCommands;
using ApplicationLogic.Querying.Queries.ConversationQueries;
using CustomMediator;
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

        [HttpPost("{bookingId:guid}")]
        public async Task<Guid> CreateConversation(Guid bookingId)
        {
            return await _mediator.SendAsync(new CreateConversationCommand(bookingId));
        }
        
        [HttpGet("request")]
        public async Task<IEnumerable<Conversation>> GetConversations([FromQuery] Guid? userId, [FromQuery] Guid? hostId)
        {
            var request = new ConversationsRequest(userId, hostId);
            return await _mediator.SendAsync(new GetConversationsQuery(request));
        }
        
        [HttpGet("{conversationId:guid}")]
        public async Task<Conversation> GetConversation(Guid conversationId)
        {
            return await _mediator.SendAsync(new GetConversationByIdQuery(conversationId));
        }
        
        [HttpPost("{conversationId:guid}/read")]
        public async Task MarkConversationAsRead(Guid conversationId)
        {
            await _mediator.SendAsync(new MarkConversationAsReadCommand(conversationId));
        }
        
        [HttpGet("booking/{bookingId:guid}")]
        public async Task<Conversation> GetConversationByBookingId(Guid bookingId)
        {
            return await _mediator.SendAsync(new GetConversationByBookingIdQuery(bookingId));
        }
        
        [HttpGet("listing/{listingId:guid}")]
        public async Task<IEnumerable<Conversation>> GetConversationsByListingId(Guid listingId)
        {
            return await _mediator.SendAsync(new GetConversationsByListingIdQuery(listingId));
        }

        [HttpDelete("{conversationId:guid}")]
        public async Task DeleteConversation(Guid conversationId)
        {
            await _mediator.SendAsync(new DeleteConversationCommand(conversationId));
        }
    }
}
