using Application.Interfaces;
using DataAccess.DAOS;
using Domain.Entities;

namespace Application.Repositories
{
    public class CarRentalRepository : ICarRentalRepository
    {
        public async Task<IEnumerable<CarRental>> GetCarRental()
        {
            IEnumerable<CarRental> carRentals = await CarRentalDAO.GetAllAsync(includes: CarRentalDAO.Includes);
            return carRentals;
        }

        public async Task AddCarRental(CarRental p) => await CarRentalDAO.AddAsync(p);
        public async Task UpdateCarRental(CarRental p) => await CarRentalDAO.UpdateAsync(p);
        public async Task DeleteCarRental(CarRental p) => await CarRentalDAO.DeleteAsync(p);

        public async Task<CarRental?> GetCarRental(int carId, int customerId, DateTime date)
        {
            CarRental carRental = await CarRentalDAO.GetByIdAsync(carId, customerId, date);
            return carRental;
        }

    }
}