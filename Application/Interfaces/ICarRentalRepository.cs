using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICarRentalRepository
    {
        Task AddCarRental(CarRental p);
        Task DeleteCarRental(CarRental p);
        Task<IEnumerable<CarRental>> GetCarRental();
        Task<CarRental?> GetCarRental(int carId, int customerId, DateTime date);
        Task UpdateCarRental(int carId, int customerId, DateTime date, CarRental p);
    }
}