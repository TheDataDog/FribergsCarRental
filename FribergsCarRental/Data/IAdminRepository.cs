using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IAdminRepository
    {
        Task<Admin> GetByEmailAsync(string email);
    }
}
