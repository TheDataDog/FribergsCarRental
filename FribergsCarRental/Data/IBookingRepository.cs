using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IBookingRepository
    {
        Booking Add(Booking booking);
        void Delete(Booking booking);
        IEnumerable<Booking> GetAll();
        Booking GetById(int id);
        void Update(Booking booking);
    }
}
