using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;
using ApplicationLogic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace airbnb_net.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly ReviewService _reviewService;

        public ReviewController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }
        // GET: api/Review
        [HttpGet]
        public async Task<List<ReviewModel>> Get()
        {
            return await _reviewService.GetAllReviews();
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ReviewModel> Get(string id)
        {
            return await _reviewService.GetReview(id);
        }

        // POST: api/Review
        [HttpPost]
        public async void Post([FromBody] ReviewModel model)
        {
            await _reviewService.CreateReviewAsync(model);
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] ReviewModel model)
        {
            await _reviewService.UpdateReviewAsync(id, model);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
            await _reviewService.DeleteReviewAsync(id);
        }
    }
}
