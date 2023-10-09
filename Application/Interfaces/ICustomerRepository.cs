using Domain.Entities;
using DTOS.DTOS;

namespace Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddCustomer(Customer p);
        Task DeleteCustomer(Customer p);
        Task<IEnumerable<Customer>> GetCustomer();
        Task<Customer?> GetCustomer(int id);
        Task<Customer?> SignIn(LoginDTO loginDto);
        Task UpdateCustomer(Customer p);
    }
}