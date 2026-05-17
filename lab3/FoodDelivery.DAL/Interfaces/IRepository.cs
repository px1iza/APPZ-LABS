using FoodDelivery.DAL.Entities;
namespace FoodDelivery.DAL.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Delete(T entity);
    }
}