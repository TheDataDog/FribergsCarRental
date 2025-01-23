using FribergsCarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergsCarRental.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext context;

        public AdminRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Admin admin)
        {
            await context.Admins.AddAsync(admin);
            await context.SaveChangesAsync();
        }

        public async Task<Admin> GetByEmailAsync(string email)
        {
            return await context.Admins.Include(a => a.UserRole)
                                 .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Admin> GetByIdAsync(int id)
        {
            return await context.Admins.FirstOrDefaultAsync(a => a.AdminId == id);
        }
    }
}
