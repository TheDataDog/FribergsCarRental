using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IAdminRepository
    {
        void Add(Admin admin);
        Admin GetById(int id);
        Admin GetByEmail(string email);
    }
}
