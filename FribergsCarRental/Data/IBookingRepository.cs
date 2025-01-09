using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        void Delete(Booking booking);
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
    }
}
