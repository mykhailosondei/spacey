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
    public class ReviewController : ControllerBase
    {
        private readonly ReviewQueryRepository _reviewQueryRepository;
        private readonly ReviewCommandAccess _reviewCommandAccess;
        
        public ReviewController(ReviewQueryRepository reviewQueryRepository, ReviewCommandAccess reviewCommandAccess)
        {
            _reviewQueryRepository = reviewQueryRepository;
            _reviewCommandAccess = reviewCommandAccess;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<IEnumerable<Review>> Get()
        {
            return await _reviewQueryRepository.GetAllReviews();
        }

        // GET: api/Review/5
        [HttpGet("{id:guid}", Name = "Get")]
        public async Task<Review> Get(Guid id)
        {
            return await _reviewQueryRepository.GetReviewById(id);
        }

        // POST: api/Review
        [HttpPost]
        public async Task<Guid> Post([FromBody] Review review)
        { 
            return await _reviewCommandAccess.AddReview(review);
        }

        // PUT: api/Review/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] Review review)
        {
            await _reviewCommandAccess.UpdateReview(id, review);
        }

        // DELETE: api/Review/5
        [HttpDelete("{id:guid}")]
        public async Task Delete(Guid id)
        {
            await _reviewCommandAccess.DeleteReview(id);
        }
    }
}
