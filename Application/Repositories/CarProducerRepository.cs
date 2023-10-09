using Application.Interfaces;
using DataAccess.DAOS;
using Domain.Entities;

namespace Application.Repositories
{
    public class CarProducerRepository : ICarProducerRepository
    {
        public async Task<IEnumerable<CarProducer>> GetCarProducer() => await CarProducerDAO.GetAllAsync(CarProducerDAO.Includes);
        public async Task<CarProducer?> GetCarProducer(int id) => await CarProducerDAO.GetByIdAsync(id, CarProducerDAO.Includes);
        public async Task AddCarProducer(CarProducer p) => await CarProducerDAO.AddAsync(p);
        public async Task UpdateCarProducer(CarProducer p) => await CarProducerDAO.UpdateAsync(p);
        public async Task DeleteCarProducer(CarProducer p) => await CarProducerDAO.DeleteAsync(p);
    }
}