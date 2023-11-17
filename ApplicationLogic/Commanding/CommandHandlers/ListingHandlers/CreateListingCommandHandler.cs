using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class CreateListingCommandHandler : BaseHandler, IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    
    public CreateListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listingDTO = _mapper.Map<ListingDTO>(request.Listing);
        var listing = _mapper.Map<Listing>(listingDTO);
            
        return await _listingCommandAccess.AddListing(listing);
    }
}