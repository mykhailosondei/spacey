using ApplicationDAL.DataCommandAccess;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class DeleteListingCommandHandler : BaseHandler, IRequestHandler<DeleteListingCommand>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    
    public DeleteListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
    }

    public async Task Handle(DeleteListingCommand request, CancellationToken cancellationToken)
    {
        await _listingCommandAccess.DeleteListing(request.Id);
    }
}