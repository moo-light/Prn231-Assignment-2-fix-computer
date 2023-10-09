using Application.Interfaces;
using DataAccess.DAOS;
using Domain.Entities;

namespace Application.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        public async Task<IEnumerable<Review>> GetReview() => await ReviewDAO.GetAllAsync(ReviewDAO.Includes);
        public async Task<Review?> GetReview(int carId, int customerId) => await ReviewDAO.GetByIdAsync( carId, customerId );
        public async Task AddReview(Review p) => await ReviewDAO.AddAsync(p);
        public async Task UpdateReview(Review p) => await ReviewDAO.UpdateAsync(p);
        public async Task DeleteReview(Review p) => await ReviewDAO.DeleteAsync(p);
    }
}