using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICarRepository
    {
        Task AddCar(Car p);
        Task DeleteCar(Car p);
        Task<IEnumerable<Car>> GetCar();
        Task<Car?> GetCar(int id);
        Task UpdateCar(Car p);
    }
}