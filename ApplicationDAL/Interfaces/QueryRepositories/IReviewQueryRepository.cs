using ApplicationDAL.Entities;

namespace ApplicationDAL.Interfaces.QueryRepositories;

public interface IReviewQueryRepository
{
    public Task<Review> GetReviewById(Guid id);

    public Task<IEnumerable<Review>> GetAllReviews();

    public Task<IEnumerable<Review>> GetReviewsByUserId(Guid userId);

    public Task<IEnumerable<Review>> GetReviewsByListingId(Guid listingId);
}