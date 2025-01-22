using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface ICarRepository
    {
        void Add(Car car);
        void Update(Car car);
        void Delete(Car car);
        Car GetById(int id);
        IEnumerable<Car> GetAll();
        IEnumerable<Car> GetAllActive();
    }
}
