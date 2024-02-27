using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.MessageCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.MessageHandlers;

public class SendMessageToHostCommandHandler : BaseHandler, IRequestHandler<SendMessageToHostCommand>
{
    
    public SendMessageToHostCommandHandler(IMapper mapper) : base(mapper)
    {
        
    }
    
    
    public Task Handle(SendMessageToHostCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

}