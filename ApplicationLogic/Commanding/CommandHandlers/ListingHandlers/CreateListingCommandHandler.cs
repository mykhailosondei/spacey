using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Exceptions;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Options;
using AutoMapper;
using BingMapsRESTToolkit;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver.GeoJsonObjectModel;

namespace ApplicationLogic.Commanding.CommandHandlers.ListingHandlers;

public class CreateListingCommandHandler : BaseHandler, IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IListingCommandAccess _listingCommandAccess;
    private readonly IHostQueryRepository _hostQueryRepository;
    private readonly IPublisher _publisher;
    private readonly BingMapsConnectionOptions _bingMapsConnectionOptions;
    
    public CreateListingCommandHandler(IMapper mapper, IListingCommandAccess listingCommandAccess, IHostQueryRepository hostQueryRepository, IPublisher publisher, IOptions<BingMapsConnectionOptions> bingMapsConnectionOptions) : base(mapper)
    {
        _listingCommandAccess = listingCommandAccess;
        _hostQueryRepository = hostQueryRepository;
        _publisher = publisher;
        _bingMapsConnectionOptions = bingMapsConnectionOptions.Value;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listingDTO = _mapper.Map<ListingDTO>(request.Listing, options => options.Items["BingMapsKey"] = _bingMapsConnectionOptions.BingMapsKey);
        
        var listing = _mapper.Map<Listing>(listingDTO);
        
        var host = await _hostQueryRepository.GetHostById(request.Listing.HostId);
        
        if (host == null)
        {
            throw new NotFoundException("Host");
        }

        GeocodeRequest geoRequest = new GeocodeRequest()
        {
            Address = new SimpleAddress()
            {
                AddressLine = listingDTO.Address.Street,
                AdminDistrict = listingDTO.Address.City,
                CountryRegion = listingDTO.Address.Country
            },
            BingMapsKey = _bingMapsConnectionOptions.BingMapsKey
        };
        
        var response = await geoRequest.Execute();
        var boundingBox = response.ResourceSets[0].Resources[0].BoundingBox;
        
        var xCoordinate = (boundingBox[3]+boundingBox[1])/2;
        
        var yCoordinate = (boundingBox[2]+boundingBox[0])/2;

        listing.Location = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(xCoordinate, yCoordinate));
        Console.WriteLine(listing.Location.Coordinates.Y);
        Console.WriteLine(listing.Location.Coordinates.X);
        
        await _publisher.Publish(new ListingCreatedEvent()
        {
            ListingId = listing.Id,
            HostId = listing.Host.Id,
            CreatedAt = DateTime.UtcNow
        });
        
        return await _listingCommandAccess.AddListing(listing);
    }
}