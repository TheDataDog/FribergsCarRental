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
        public async Task<Customer> AddAsync(Customer customer)
        {
            await context.Customers.AddAsync(customer);
            await context.SaveChangesAsync();
            return customer;
        }

        public async Task DeleteAsync(Customer customer)
        {
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await context.Customers.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await context.Customers.Include(c => c.UserRole).FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await context.Customers.Include(b=>b.Bookings).Include(c => c.Adress)
                                    .Include(c => c.UserRole).FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer> GetByIdIncludeBookingsAsync(int id)
        {
            return await context.Customers.Include(b => b.Bookings).ThenInclude(b => b.Car) //något fel med den här???
                                    .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            context.Customers.Update(customer);
            await context.SaveChangesAsync();
        }
    }
}
