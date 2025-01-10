using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext context;

        public CustomerRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            context.Customers.Remove(customer);
            context.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return context.Customers.OrderBy(c => c.LastName);
        }

        public Customer GetByEmail(string email)
        {
            //return context.Customers.FirstOrDefault(c => c.Email == email);
            return context.Customers.Include(c => c.UserRole).FirstOrDefault(c => c.Email == email);
        }

        public Customer GetById(int id)
        {
            //return context.Customers.FirstOrDefault(c => c.CustomerId == id);
            return context.Customers.Include(c => c.Adress).Include(c => c.UserRole).FirstOrDefault(c => c.CustomerId == id);
        }

        public void Update(Customer customer)
        {
            context.Customers.Update(customer);
            context.SaveChanges();
        }
    }
}
