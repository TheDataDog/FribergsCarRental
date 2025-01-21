using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class CarRepository : GenericRepository<Car>
    {

        public CarRepository(ApplicationDbContext context) : base(context) { }

        public override async Task<Car> GetByIdAsync(int id)
        {
            return await context.Cars.Include(c => c.Bookings)
                                     .FirstOrDefaultAsync(c => c.CarId == id);
        }

    }
}
