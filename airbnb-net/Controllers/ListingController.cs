using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly ListingService _listingService;
        private readonly ReviewService _reviewService;

        public ListingController(ListingService listingService)
        {
            _listingService = listingService;
        }
        // GET: api/Listing
        [HttpGet]
        public async Task<List<ListingModel>> Get()
        {
            return await _listingService.GetAllListingsAsync();
        }

        // GET: api/Listing/5
        [HttpGet("{id}")]
        public async Task<ListingModel> Get(string id)
        {
            return await _listingService.GetListingAsync(id);
        }
        
        // GET: api/Listing/5/Review
        [HttpGet("{id}/Review")]
        public async Task<List<ReviewModel>> GetReviews(string id)
        {
            return await _reviewService.GetAllReviewsByListing(id);
        }

        // POST: api/Listing
        [HttpPost]
        public async void Post([FromBody] ListingModel value)
        {
            await _listingService.CreateListingAsync(value);
        }

        // PUT: api/Listing/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] ListingModel model)
        {
            //Console.WriteLine("bop");
            await _listingService.UpdateListingAsync(id, model);
        }

        // DELETE: api/Listing/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _listingService.DeleteListingAsync(id);
        }
    }
}
