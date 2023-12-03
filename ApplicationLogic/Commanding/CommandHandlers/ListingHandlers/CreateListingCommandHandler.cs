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

public class CreateListingCommandHandler : BaseHandler, IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IPublisher _publisher;
    
    public CreateListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess, IHostQueryRepository hostQueryRepository, IPublisher publisher) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
        _hostQueryRepository = hostQueryRepository;
        _publisher = publisher;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listingDTO = _mapper.Map<ListingDTO>(request.Listing);
        var listing = _mapper.Map<Listing>(listingDTO);
        
        var host = await _hostQueryRepository.GetHostById(request.Listing.HostId);
        
        if (host == null)
        {
            throw new NotFoundException("Host");
        }
        
        await _publisher.Publish(new ListingCreatedEvent()
        {
            ListingId = listing.Id,
            HostId = listing.Host.Id,
            CreatedAt = DateTime.UtcNow
        });
        
        return await _listingCommandAccess.AddListing(listing);
    }
}