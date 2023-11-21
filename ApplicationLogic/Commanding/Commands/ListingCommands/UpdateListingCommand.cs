using ApplicationCommon.DTOs.Listing;
using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record UpdateListingCommand(Guid Id, ListingUpdateDTO Listing) : IRequest, ICommand;