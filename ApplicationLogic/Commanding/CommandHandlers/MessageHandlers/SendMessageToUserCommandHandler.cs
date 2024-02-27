using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.MessageCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.MessageHandlers;

public class SendMessageToUserCommandHandler : BaseHandler ,IRequestHandler<SendMessageToUserCommand>
{
    public SendMessageToUserCommandHandler(IMapper mapper) : base(mapper)
    {
    }

    public Task Handle(SendMessageToUserCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}