using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record UnlikeListingCommand(Guid Id) : IRequest;