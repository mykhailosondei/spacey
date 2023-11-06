namespace ApplicationDAL.Interfaces;

public interface IReviewDeletor
{
    public Task DeleteReview(Guid id);
}