using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface ICustomerRepository
    {
        Task<Customer> AddAsync(Customer customer);
        Task UpdateAsync(Customer customer);
        Task DeleteAsync(Customer customer);
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByEmailAsync(string email);
        Task<Customer> GetByIdIncludeBookingsAsync(int id);
    }
}
