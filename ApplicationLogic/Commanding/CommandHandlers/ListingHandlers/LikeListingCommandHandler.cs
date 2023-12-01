using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class LikeListingCommandHandler : BaseHandler, IRequestHandler<LikeListingCommand>
{
    private readonly IListingQueryRepository _listingQueryRepository;
    private readonly IUserIdGetter _userIdGetter;
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IUserQueryRepository _userQueryRepository;
    private readonly IUserCommandAccess _userCommandAccess;
    
    public LikeListingCommandHandler(IMapper mapper, IListingQueryRepository listingQueryRepository, IUserIdGetter userIdGetter, IListingCommandAccess listingCommandAccess, IUserQueryRepository userQueryRepository, IUserCommandAccess userCommandAccess) : base(mapper)
    {
        _listingQueryRepository = listingQueryRepository;
        _userIdGetter = userIdGetter;
        _listingCommandAccess = listingCommandAccess;
        _userQueryRepository = userQueryRepository;
        _userCommandAccess = userCommandAccess;
    }

    public async Task Handle(LikeListingCommand request, CancellationToken cancellationToken)
    {
        var listing = await _listingQueryRepository.GetListingById(request.Id);
        
        var user = await _userQueryRepository.GetUserById(_userIdGetter.UserId);
        
        if (listing == null)
        {
            throw new NotFoundException("Listing");
        }

        if (listing.LikedUsersIds.Contains(user.Id))
        {
            throw new InvalidOperationException("You have already liked this listing.");
        }
        
        listing.LikedUsersIds.Add(user.Id);
        user.LikedListingsIds.Add(listing.Id);
        
        await _listingCommandAccess.UpdateListing(listing.Id ,listing);
        await _userCommandAccess.UpdateUser(user.Id, user);
    }
}