using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class BookingRepository : GenericRepository<Booking>
    {

        public BookingRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Booking> GetByIdAsync(int id)
        {
            return await context.Bookings.Include(b => b.Car)
                                         .Include(b => b.Customer)
                                         .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public override async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await context.Bookings.Include(b => b.Car)
                                         .Include(b => b.Customer)
                                         .OrderByDescending(b => b.StartDate)
                                         .ToListAsync();
        }
    }
}
