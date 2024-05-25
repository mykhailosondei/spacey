using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record LikeListingCommand(Guid Id) : IRequest; 