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
        public async Task AddAsync(Car car)
        {
            await context.Cars.AddAsync(car);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Car car)
        {
            context.Cars.Remove(car);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Car>> GetAllAsync()
        {
            return await context.Cars.OrderBy(c => c.Brand).ToListAsync();
        }

        public async Task<IEnumerable<Car>> GetAllActiveAsync()
        {
            return await context.Cars.Where(c => c.IsActive == true).OrderBy(c => c.Brand).ToListAsync();
        }

        public async Task<Car> GetByIdAsync(int id)
        {
            return await context.Cars.Include(c => c.Bookings).FirstOrDefaultAsync(c => c.CarId == id);
        }

        public async Task UpdateAsync(Car car)
        {
            context.Cars.Update(car);
            await context.SaveChangesAsync();
        }
    }
}
