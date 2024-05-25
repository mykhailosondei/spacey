using ApplicationCommon.DTOs.Listing;
using CustomMediator;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record CreateListingCommand(ListingCreateDTO Listing) : IRequest<Guid>, ICommand;