using Amazon.Runtime.Internal;
using MediatR;
using IRequest = MediatR.IRequest;

namespace ApplicationLogic.Commanding.Commands.ReviewCommands;

public record DeleteReviewCommand(Guid Id) : IRequest, ICommand;