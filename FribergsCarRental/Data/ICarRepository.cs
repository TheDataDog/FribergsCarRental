using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface ICarRepository
    {
        Task AddAsync(Car car);
        Task UpdateAsync(Car car);
        Task DeleteAsync(Car car);
        Task<Car> GetByIdAsync(int id);
        Task<IEnumerable<Car>> GetAllAsync();
        Task<IEnumerable<Car>> GetAllActiveAsync();
    }
}
