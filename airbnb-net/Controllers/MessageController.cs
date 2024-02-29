using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Message;
using ApplicationLogic.Commanding.Commands.MessageCommands;
using ApplicationLogic.RoleLogic;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IRoleGetter _roleGetter;

        public MessageController(IMediator mediator, IRoleGetter roleGetter)
        {
            _mediator = mediator;
            _roleGetter = roleGetter;
        }
        
        [HttpPost("{conversationId:guid}")]
        [Authorize(Roles = "Host, User")]
        public async Task SendMessage(Guid conversationId, [FromBody] MessageCreateDTO messageCreate)
        {
            Console.WriteLine(string.Join(", ", _roleGetter.Roles));
            var isHost = _roleGetter.IsInRole("Host");
            var task = isHost switch {
                true =>  _mediator.Send(new SendMessageToUserCommand(conversationId, messageCreate.MessageContent)),
                false => _mediator.Send(new SendMessageToHostCommand(conversationId, messageCreate.MessageContent))
            };
            await task;
        }
    }
}
