using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext context;

        public CarRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public void Add(Car car)
        {
            context.Cars.Add(car);
            context.SaveChanges();
        }

        public void Delete(Car car)
        {
            context.Cars.Remove(car);
            context.SaveChanges();
        }

        public IEnumerable<Car> GetAll()
        {
            return context.Cars.OrderBy(c => c.Brand);
        }

        public IEnumerable<Car> GetAllActive()
        {
            return context.Cars.Where(c => c.IsActive == true).OrderBy(c => c.Brand);
        }

        public Car GetById(int id)
        {
            return context.Cars.Include(c => c.Bookings).FirstOrDefault(c => c.CarId == id);
        }

        public void Update(Car car)
        {
            context.Cars.Update(car);
            context.SaveChanges();
        }
    }
}
