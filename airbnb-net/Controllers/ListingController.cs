using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly ListingQueryRepository _listingQueryRepository;
        private readonly ListingCommandAccess _listingCommandAccess;
        
        public ListingController(ListingQueryRepository listingQueryRepository, ListingCommandAccess listingCommandAccess)
        {
            _listingQueryRepository = listingQueryRepository;
            _listingCommandAccess = listingCommandAccess;
        }
        
        // GET: api/Listing
        [HttpGet]
        public async Task<IEnumerable<Listing>> Get()
        {
            return await _listingQueryRepository.GetAllListings();
        }
        
        // GET: api/Listing/5
        [HttpGet("{id:guid}")]
        public async Task<Listing> Get(Guid id)
        {
            return await _listingQueryRepository.GetListingById(id);
        }
        
        // POST: api/Listing
        [HttpPost]
        public async Task Post([FromBody] Listing listing)
        {
            await _listingCommandAccess.AddListing(listing);
        }
        
        // PUT: api/Listing/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] Listing listing)
        {
            await _listingCommandAccess.UpdateListing(id, listing);
        }
        
        // DELETE: api/Listing/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _listingCommandAccess.DeleteListing(id);
        }
    }
}
