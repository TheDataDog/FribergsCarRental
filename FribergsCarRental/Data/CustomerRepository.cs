using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository(ApplicationDbContext context) : base(context) { }


        public override async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await context.Customers.OrderBy(c => c.LastName).ToListAsync();
        }

        public async Task<Customer> GetByEmailAsync(string email)
        {
            return await context.Customers.Include(c => c.UserRole)
                                          .FirstOrDefaultAsync(c => c.Email == email);
        }

        public override async Task<Customer> GetByIdAsync(int id)
        {
            return await context.Customers.Include(b=>b.Bookings)
                                          .Include(c => c.Adress)
                                          .Include(c => c.UserRole)
                                          .FirstOrDefaultAsync(c => c.CustomerId == id);
        }

        public async Task<Customer> GetByIdBookingsAsync(int id)
        {
            return await context.Customers.Include(b => b.Bookings)
                                          .ThenInclude(b => b.Car) //något fel med den här???
                                          .FirstOrDefaultAsync(c => c.CustomerId == id);
        }
    }
}
