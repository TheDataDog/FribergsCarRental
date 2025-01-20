using FribergsCarRental.Models;

namespace FribergsCarRental.Data
{
    public interface IRepository<T>
    {
        T Add(T entity);
        T Update(T entity);
        void Delete(T car);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
