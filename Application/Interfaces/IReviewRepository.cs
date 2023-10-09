using Domain.Entities;

namespace Application.Interfaces
{
    public interface IReviewRepository
    {
        Task AddReview(Review p);
        Task DeleteReview(Review p);
        Task<IEnumerable<Review>> GetReview();
        Task<Review?> GetReview(int carId, int customerId);
        Task UpdateReview(Review p);
    }
}