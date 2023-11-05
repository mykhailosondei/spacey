using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCommon.DTOs.Listing;
using ApplicationDAL.DataCommandAccess;
using ApplicationDAL.DataQueryAccess;
using ApplicationDAL.Entities;
using AutoMapper;
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
        private readonly IMapper _mapper; 
        
        public ListingController(ListingQueryRepository listingQueryRepository, ListingCommandAccess listingCommandAccess, IMapper mapper)
        {
            _listingQueryRepository = listingQueryRepository;
            _listingCommandAccess = listingCommandAccess;
            _mapper = mapper;
        }
        
        // GET: api/Listing
        [HttpGet]
        public async Task<IEnumerable<ListingDTO>> Get()
        {
            var result = await _listingQueryRepository.GetAllListings();
            return _mapper.Map<IEnumerable<ListingDTO>>(result);
        }
        
        // GET: api/Listing/5
        [HttpGet("{id:guid}")]
        public async Task<ListingDTO> Get(Guid id)
        {
            return _mapper.Map<ListingDTO>(await _listingQueryRepository.GetListingById(id));
        }
        
        // POST: api/Listing
        [HttpPost]
        public async Task Post([FromBody] ListingCreateDTO listing)
        {
            var listingEntity = _mapper.Map<Listing>(listing);
            await _listingCommandAccess.AddListing(listingEntity);
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
