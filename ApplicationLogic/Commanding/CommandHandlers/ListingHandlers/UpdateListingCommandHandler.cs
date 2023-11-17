using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class UpdateListingCommandHandler : BaseHandler, IRequestHandler<UpdateListingCommand>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    
    public UpdateListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
    }

    public async Task Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var listingDTO = _mapper.Map<ListingDTO>(request.Listing);
        var listing = _mapper.Map<Listing>(listingDTO);
            
        await _listingCommandAccess.UpdateListing(request.Id ,listing);
    }
}