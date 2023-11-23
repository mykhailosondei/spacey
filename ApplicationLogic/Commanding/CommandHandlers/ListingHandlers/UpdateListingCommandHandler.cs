using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class UpdateListingCommandHandler : BaseHandler, IRequestHandler<UpdateListingCommand>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    
    public UpdateListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess, IListingQueryRepository listingQueryRepository, IHostIdGetter hostIdGetter) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
        _listingQueryRepository = listingQueryRepository;
        _hostIdGetter = hostIdGetter;
    }

    public async Task Handle(UpdateListingCommand request, CancellationToken cancellationToken)
    {
        var listingDTO = _mapper.Map<ListingDTO>(request.Listing);
        var listing = _mapper.Map<Listing>(listingDTO);
            
        var existingListing = await _listingQueryRepository.GetListingById(request.Id);
        
        if (existingListing == null)
        {
            throw new NotFoundException("Listing");
        }
        
        if(existingListing.Host.Id != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to update this listing.");
        }
        
        await _listingCommandAccess.UpdateListing(request.Id ,listing);
    }
}