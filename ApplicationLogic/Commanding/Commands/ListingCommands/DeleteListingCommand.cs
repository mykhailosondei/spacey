using MediatR;

namespace ApplicationLogic.Commanding.Commands.ListingCommands;

public record DeleteListingCommand(Guid Id) : IRequest;