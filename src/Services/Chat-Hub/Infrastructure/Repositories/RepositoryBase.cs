
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ChatAppContext DbContext;

        public RepositoryBase(ChatAppContext dbxContext)
        {
            DbContext = dbxContext;
        }
        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await DbContext.Set<T>().ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbContext.Set<T>().Where(predicate).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
                                                     string? includeString = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = DbContext.Set<T>();
            if (disableTracking) query = query.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) return await orderBy(query).ToListAsync();

            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().FindAsync(id); 
        }

        public async Task<T> AddAsync(T entity)
        {
            DbContext.Set<T>().Add(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            DbContext.Set<T>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}
