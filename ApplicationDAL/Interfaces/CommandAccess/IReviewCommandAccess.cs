using ApplicationDAL.Entities;

namespace ApplicationDAL.DataCommandAccess;

public interface IReviewCommandAccess
{
    public Task<Guid> AddReview(Review review);
    public Task UpdateReview(Guid id, Review review);
    public Task DeleteReview(Guid id);
}