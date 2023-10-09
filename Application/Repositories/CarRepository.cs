using Application.Interfaces;
using DataAccess.DAOS;
using Domain.Entities;

namespace Application.Repositories
{
    public class CarRepository : ICarRepository
    {
        public async Task<IEnumerable<Car>> GetCar() => await CarDAO.GetAllAsync(CarDAO.Includes);
        public async Task<Car?> GetCar(int id) => await CarDAO.GetByIdAsync(id, CarDAO.Includes);
        public async Task AddCar(Car p) => await CarDAO.AddAsync(p);
        public async Task UpdateCar(Car p) => await CarDAO.UpdateAsync(p);
        public async Task DeleteCar(Car p) => await CarDAO.DeleteAsync(p);
    }
}