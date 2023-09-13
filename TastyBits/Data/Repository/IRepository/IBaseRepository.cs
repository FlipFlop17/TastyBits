using System.Linq.Expressions;

namespace TastyBits.Data.Repository.IRepository
{
    public interface IBaseRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, bool asNoTracking = false);
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false);
        Task<T> InsertAsync(T entity);
        Task<T> UpdateAsync(int id);
        Task DeleteAsync(int id);
    }
}
