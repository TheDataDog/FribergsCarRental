using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface ICustomerRepository
    {
        Customer Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        Customer GetByEmail(string email);
    }
}
