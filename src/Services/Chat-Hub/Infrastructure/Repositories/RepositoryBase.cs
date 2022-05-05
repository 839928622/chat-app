
using Application.Contracts.Persistence;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : class
    {
        protected readonly ChatAppContext ChatAppDbContext;

        public RepositoryBase(ChatAppContext dbxContext)
        {
            ChatAppDbContext = dbxContext;
        }
        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await ChatAppDbContext.Set<T>().ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await ChatAppDbContext.Set<T>().Where(predicate).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>>? predicate,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy,
                                                     string? includeString = null,
                                                     bool disableTracking = true)
        {
            IQueryable<T> query = ChatAppDbContext.Set<T>();
           
            if (!string.IsNullOrWhiteSpace(includeString)) query = query.Include(includeString);
            if (predicate != null) query = query.Where(predicate);
            if (orderBy != null) return await orderBy(query).ToListAsync();

            if (disableTracking) query = query.AsNoTracking();
            return await query.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await ChatAppDbContext.Set<T>().FindAsync(id); 
        }

        public async Task<T> AddAsync(T entity)
        {
            ChatAppDbContext.Set<T>().Add(entity);
            await ChatAppDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            ChatAppDbContext.Entry(entity).State = EntityState.Modified;
            await ChatAppDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            ChatAppDbContext.Set<T>().Remove(entity);
            await ChatAppDbContext.SaveChangesAsync();
        }
    }
}
