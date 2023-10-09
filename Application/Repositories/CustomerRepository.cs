using Application.Interfaces;
using Application.Utils;
using DataAccess.DAOS;
using DTOS.DTOS;
using Domain.Entities;

namespace Application.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        public async Task<IEnumerable<Customer>> GetCustomer() => await CustomerDAO.GetAllAsync(CustomerDAO.Includes);
        public async Task<Customer?> GetCustomer(int id) => await CustomerDAO.GetByIdAsync(id, CustomerDAO.Includes);
        public async Task AddCustomer(Customer p) => await CustomerDAO.AddAsync(p);
        public async Task UpdateCustomer(Customer p) => await CustomerDAO.UpdateAsync(p);
        public async Task DeleteCustomer(Customer p) => await CustomerDAO.DeleteAsync(p);

        public async Task<Customer?> SignIn(LoginDTO loginDto)
        {
            //Admin login
            if (loginDto.Equals(MyTools.GetAdminAccount())) return new Customer
            {
                Id = 0,
                CustomerName = "Admin",
                Role = "Admin",
            };
            //Customer Login
            var customers = await CustomerDAO.GetAllAsync();
            var signedCustomer = customers.FirstOrDefault(x => x.Email.Equals(loginDto.Email, StringComparison.OrdinalIgnoreCase));
            if (signedCustomer != null && signedCustomer.Password != loginDto.Password) signedCustomer = null;
            return signedCustomer;
        }
    }
}