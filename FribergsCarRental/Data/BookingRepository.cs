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
            return context.Bookings.Include(b => b.Car).Include(b => b.Customer).OrderByDescending(b => b.StartDate);
        }

        public Booking GetById(int id)
        {
            return context.Bookings.FirstOrDefault(b => b.BookingId == id);
        }

        //public Booking New(int customerId)
        //{
        //    var booking = new Booking
        //    {
        //        CarId = (int)sessionHelper.GetCarSession(),
        //        CustomerId = (int)customerId //kan skrivas snyggare direkt från GetUserSession?
        //        Customer = context.Customers.

        //    };
        //    return booking;
        //}
    }
}
