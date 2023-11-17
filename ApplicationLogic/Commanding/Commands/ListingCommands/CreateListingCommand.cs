using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record CreateListingCommand(ListingCreateDTO Listing) : IRequest<Guid>;