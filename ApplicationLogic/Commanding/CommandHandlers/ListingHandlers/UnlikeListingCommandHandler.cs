using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class UnlikeListingCommandHandler : BaseHandler, IRequestHandler<UnlikeListingCommand>
{
    
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandAccess _userCommandAccess;
    private readonly IPublisher _publisher;

    public UnlikeListingCommandHandler(IMapper mapper, IListingQueryRepository listingQueryRepository, IUserIdGetter userIdGetter, IListingCommandAccess listingCommandAccess, IUserQueryRepository userQueryRepository, IUserCommandAccess userCommandAccess, IPublisher publisher) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
        _userIdGetter = userIdGetter;
        _listingCommandAccess = listingCommandAccess;
        _userQueryRepository = userQueryRepository;
        _userCommandAccess = userCommandAccess;
        _publisher = publisher;
    }

    public async Task Handle(UnlikeListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _listingQueryRepository.GetListingById(request.Id);
        
        var user = await _userQueryRepository.GetUserById(_userIdGetter.UserId);
        
        if (listing == null)
        {
            throw new NotFoundException("Listing");
        }
        
        if (!listing.LikedUsersIds.Contains(user.Id))
        {
            throw new InvalidOperationException("You have not liked this listing.");
        }
        
        listing.LikedUsersIds.Remove(user.Id);
        user.LikedListingsIds.Remove(listing.Id);
        
        await _listingCommandAccess.UpdateListing(listing.Id ,listing);
        await _userCommandAccess.UpdateUser(user.Id, user);
        
        await _publisher.Publish(new ListingUpdatedEvent()
        {
            ListingId = listing.Id,
            HostId = listing.Host.Id,
            UpdatedAt = DateTime.UtcNow
        });
    }
}