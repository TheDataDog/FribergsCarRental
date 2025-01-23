using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IBookingRepository
    {
        Task<Booking> AddAsync(Booking booking);
        Task DeleteAsync(Booking booking);
        Task<IEnumerable<Booking>> GetAllAsync();
        Task<Booking> GetByIdAsync(int id);
        Task UpdateAsync(Booking booking);
    }
}
