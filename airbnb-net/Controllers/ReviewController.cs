using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using airbnb_net.Controllers.Abstract;
using ApplicationCommon.DTOs.Review;
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
    public class ReviewController : InternalControllerBase
    {
        private readonly ReviewQueryRepository _reviewQueryRepository;
        private readonly ReviewCommandAccess _reviewCommandAccess;
        
        public ReviewController(ReviewQueryRepository reviewQueryRepository, ReviewCommandAccess reviewCommandAccess, ILogger<InternalControllerBase> logger, IMapper mapper)
        : base(logger, mapper)
        {
            _reviewQueryRepository = reviewQueryRepository;
            _reviewCommandAccess = reviewCommandAccess;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<IEnumerable<ReviewDTO>> Get()
        {
            return _mapper.Map<IEnumerable<ReviewDTO>>(await _reviewQueryRepository.GetAllReviews());
        }

        // GET: api/Review/5
        [HttpGet("{id:guid}", Name = "Get")]
        public async Task<ReviewDTO> Get(Guid id)
        {
            return _mapper.Map<ReviewDTO>(await _reviewQueryRepository.GetReviewById(id));
        }

        // POST: api/Review
        [HttpPost]
        public async Task<Guid> Post([FromBody] ReviewCreateDTO reviewCreate)
        { 
            var reviewDTO = _mapper.Map<ReviewDTO>(reviewCreate);
            var review = _mapper.Map<Review>(reviewDTO);
            return await _reviewCommandAccess.AddReview(review);
        }

        // PUT: api/Review/5
        [HttpPut("{id:guid}")]
        public async Task Put(Guid id, [FromBody] ReviewUpdateDTO reviewUpdate)
        {
            var reviewDTO = _mapper.Map<ReviewDTO>(reviewUpdate);
            var review = _mapper.Map<Review>(reviewDTO);
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
