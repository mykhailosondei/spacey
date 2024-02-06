using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Listing;
using ApplicationCommon.GeospatialUtilities;
using ApplicationCommon.Structs;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using ApplicationDAL.Interfaces.QueryRepositories;
using ApplicationLogic.Abstract;
using ApplicationLogic.Commanding.Commands.ListingCommands;
using ApplicationLogic.Filters;
using ApplicationLogic.Filters.Abstract;
using ApplicationLogic.HostIdLogic;
using ApplicationLogic.Querying.Queries.BookingQueries;
using ApplicationLogic.Querying.Queries.ListingQueries;
using ApplicationLogic.UserIdLogic;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : InternalControllerBase
    {
        private readonly IHostIdGetter _hostIdGetter;
        private readonly IMediator _mediator;

        public ListingController(ILogger<InternalControllerBase> logger, IMapper mapper, IUserIdGetter userIdGetter, IMediator mediator, IHostIdGetter hostIdGetter) : base(logger,mapper)
        {
            _mediator = mediator;
            _hostIdGetter = hostIdGetter;
        }
        
        // GET: api/Listing
        [HttpGet]
        public async Task<IEnumerable<ListingDTO>> Get()
        {
            return await _mediator.Send(new GetAllListingsQuery());
        }
        
        // GET: api/Listing/5
        [HttpGet("{id:guid}")]
        public async Task<ListingDTO> Get(Guid id)
        {
            return await _mediator.Send(new GetListingByIdQuery(id));
        }
        
        [HttpGet("{id:guid}/distance")]
        public async Task<double> GetDistance(Guid id, double latitude, double longitude)
        {
            var coordinates = new Coordinates(longitude, latitude);
            Console.WriteLine("Coordinates: " + coordinates.Latitude + " " + coordinates.Longitude);
            return await _mediator.Send(new GetDistanceToListingQuery(id, coordinates));
        }
        
        // GET: api/Listing/5/unavailableDates
        [HttpGet("{id:guid}/unavailableDates")]
        public async Task<IEnumerable<DateTime>> GetUnavailableDates(Guid id)
        {
            var unavailableDates = new List<DateTime>();
            var listing = await _mediator.Send(new GetListingByIdQuery(id));
            foreach (var bookingId in listing.BookingsIds)
            {
                var booking = await _mediator.Send(new GetBookingByIdQuery(bookingId));
                for (DateTime i = booking.CheckIn; i <= booking.CheckOut; i = i.AddDays(1))
                {
                    unavailableDates.Add(i);
                }
            }
            return unavailableDates;
        }
        
        [HttpGet("propertyType/{propertyType}")]
        public async Task<IEnumerable<ListingDTO>> Get(string propertyType)
        {
            return await _mediator.Send(new GetListingsByPropertyTypeQuery(propertyType));
        }
        
        [HttpGet("boundingBox")]
        public async Task<IEnumerable<ListingDTO>> Get(double x1, double y1, double x2, double y2)
        {
            var boundingBox = new BoundingBox(new Coordinates(x1, y1), new Coordinates(x2, y2));
            return await _mediator.Send(new GetListingsByBoundingBoxQuery(boundingBox));
        }
        
        [HttpGet("address")]
        public async Task<IEnumerable<ListingDTO>> Get(string? city, string? country, string? street)
        {
            var address = new Address
            {
                City = city,
                Country = country,
                Street = street
            };
            return await _mediator.Send(new GetListingsByAddressQuery(address));
        }
        
        [HttpGet("search")]
        public async Task<IEnumerable<ListingDTO>> Get(string place, DateTime checkIn, DateTime checkOut, int guests)
        {
            return await _mediator.Send(new GetListingsBySearchQuery()
            {
                Filters = new List<AbstractFilter>()
                {
                    new PlaceFilter(place),
                    new DateFilter(checkIn, checkOut),
                    new GuestsFilter(guests)
                }
            });
        }
        
        // POST: api/Listing
        [HttpPost]
        [Authorize(Roles = "Host")]
        public async Task<Guid> Post([FromBody] ListingCreateDTO listingCreate)
        {
            listingCreate.HostId = _hostIdGetter.HostId;
            _logger.LogInformation("HostId:" + listingCreate.HostId.ToString());
            return await _mediator.Send(new CreateListingCommand(listingCreate));
        }
        
        // PUT: api/Listing/5
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Host")]
        public async Task Put(Guid id, [FromBody] ListingUpdateDTO listingUpdate)
        {
            await _mediator.Send(new UpdateListingCommand(id, listingUpdate));
        }
        
        // DELETE: api/Listing/5
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Host")]
        public async Task Delete(Guid id)
        {
            await _mediator.Send(new DeleteListingCommand(id));
        }
        
        // POST: api/Listing/123/like
        [HttpPost("{id:guid}/like")]
        [Authorize(Roles = "User")]
        public async Task Like(Guid id)
        {
            await _mediator.Send(new LikeListingCommand(id));
        }
        
        // POST: api/Listing/123/unlike
        [HttpPost("{id:guid}/unlike")]
        [Authorize(Roles = "User")]
        public async Task Unlike(Guid id)
        {
            await _mediator.Send(new UnlikeListingCommand(id));
        }
    }
}
