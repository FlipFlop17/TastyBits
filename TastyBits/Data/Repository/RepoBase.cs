using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Linq.Expressions;
using TastyBits.Data.Repository.IRepository;

namespace TastyBits.Data.Repository
{
    public class RepoBase<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _appDb;
        private DbSet<T> _dbSet;

        public RepoBase(AppDbContext appDb) 
        {
            _appDb = appDb;
            this._dbSet = _appDb.Set<T>();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, bool asNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            throw new NotImplementedException();
        }

        public async Task<T> InsertAsync(T entity)
        {
            _dbSet.Add(entity);
            await _appDb.SaveChangesAsync();
            return entity;
        }

        public Task<T> UpdateAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
