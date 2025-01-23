using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IAdminRepository
    {
        Task AddAsync(Admin admin);
        Task<Admin> GetByIdAsync(int id);
        Task<Admin> GetByEmailAsync(string email);
    }
}
