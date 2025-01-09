using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext context;

        public BookingRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add(Booking booking)
        {
            context.Bookings.Add(booking);
            context.SaveChanges();
        }

        public void Delete(Booking booking)
        {
            context.Bookings.Remove(booking);
            context.SaveChanges();
        }

        public IEnumerable<Booking> GetAll()
        {
            return context.Bookings.OrderBy(b => b.Start);
        }

        public Booking GetById(int id)
        {
            return context.Bookings.FirstOrDefault(b => b.BookingId == id);
        }
    }
}
