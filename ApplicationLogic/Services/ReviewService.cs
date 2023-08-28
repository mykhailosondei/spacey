using ApplicationDataAccess.DataAccess;
using ApplicationDataAccess.Models;

namespace ApplicationLogic.Services;

public class ReviewService : BaseService
{
    public ReviewService(DataAccessManager dataAccessManager) : base(dataAccessManager)
    {
        
    }

    public async Task<List<ReviewModel>> GetAllReviews()
    {
        return await _dataAccessManager.GetAllReviews();
    }
    
    public async Task<ReviewModel> GetReview(string id)
    {
        return await _dataAccessManager.GetReview(id);
    }

    public async Task CreateReviewAsync(ReviewModel model)
    {
        await _dataAccessManager.CreateReviewAsync(model);
    }

    public async Task UpdateReviewAsync(string id, ReviewModel model)
    {
        await _dataAccessManager.UpdateReviewAsync(id, model);
    }
    
    public async Task<List<ReviewModel>> GetAllReviewsByListing(string id)
    {
        return await _dataAccessManager.GetAllReviewsByListing(id);
    }

    public async Task DeleteReviewAsync(string id)
    {
        await _dataAccessManager.DeleteReviewAsync(id);
    }
}