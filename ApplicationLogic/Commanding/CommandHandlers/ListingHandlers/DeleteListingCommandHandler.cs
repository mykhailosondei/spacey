using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class DeleteListingCommandHandler : BaseHandler, IRequestHandler<DeleteListingCommand>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IHostIdGetter _hostIdGetter;
    
    public DeleteListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess, IListingQueryRepository listingQueryRepository, IHostIdGetter hostIdGetter) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
        _listingQueryRepository = listingQueryRepository;
        _hostIdGetter = hostIdGetter;
    }

    public async Task Handle(DeleteListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _listingQueryRepository.GetListingById(request.Id);
        
        if (listing == null)
        {
            throw new NotFoundException("Listing");
        }
        
        if(listing.Host.Id != _hostIdGetter.HostId)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this listing.");
        }
        
        await _listingCommandAccess.DeleteListing(request.Id);
    }
}