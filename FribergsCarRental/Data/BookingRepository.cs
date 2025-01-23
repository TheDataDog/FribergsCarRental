using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext context;

        public BookingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Booking> AddAsync(Booking booking)
        {
            await context.Bookings.AddAsync(booking);
            await context.SaveChangesAsync();
            return booking;
        }

        public async Task DeleteAsync(Booking booking)
        {
            context.Bookings.Remove(booking);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Booking>> GetAllAsync()
        {
            return await context.Bookings.Include(b => b.Car)
                                   .Include(b => b.Customer)
                                   .OrderByDescending(b => b.StartDate).ToListAsync();
        }

        public async Task<Booking> GetByIdAsync(int id)
        {
            return await context.Bookings.Include(b => b.Car)
                                   .Include(b => b.Customer)
                                   .FirstOrDefaultAsync(b => b.BookingId == id);
        }
        public async Task UpdateAsync(Booking booking)
        {
            context.Bookings.Update(booking);
            await context.SaveChangesAsync();
        }
    }
}
