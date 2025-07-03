using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeafoodApp.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id); // Kiểu trả về là Task<T?>
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
    }
}