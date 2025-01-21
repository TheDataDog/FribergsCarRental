using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class AdminRepository : GenericRepository<Admin>, IAdminRepository
    {
        public AdminRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await context.Admins.Include(a => a.UserRole)
                                        .FirstOrDefaultAsync(a => a.Email == email);
        }
    }
}
