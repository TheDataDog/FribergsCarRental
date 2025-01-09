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
        public void Add(Admin admin)
        {
            context.Admins.Add(admin);
            context.SaveChanges();
        }

        public Admin GetByEmail(string email)
        {
            //return context.Admins.Include(a => a.UserRole).FirstOrDefault(a => a.Email == email);
            return context.Admins.Include(a => a.UserRole)
                                 .FirstOrDefault(a => a.Email == email);
        }

        public Admin GetById(int id)
        {
            return context.Admins.FirstOrDefault(a => a.AdminId == id);
        }
    }
}
