using Domain.Entities;

namespace Application.Interfaces
{
    public interface ICarProducerRepository
    {
        Task AddCarProducer(CarProducer p);
        Task DeleteCarProducer(CarProducer p);
        Task<IEnumerable<CarProducer>> GetCarProducer();
        Task<CarProducer?> GetCarProducer(int id);
        Task UpdateCarProducer(CarProducer p);
    }
}