using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uploader.Core.Repositories.Interfaces;
using Uploader.Infrastructure.Data.Contexts;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Core.Repositories
{
    public class AsyncRepository<TEntity> : IAsyncRepository<TEntity> where TEntity : Entity
    {
        private readonly UploaderContext dbContext;
        private readonly DbSet<TEntity> entities;

        public AsyncRepository(UploaderContext dbContext)
        {
            this.dbContext = dbContext;
            entities = dbContext.Set<TEntity>();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity)
        {
            var entityEntry = await entities.AddAsync(entity).ConfigureAwait(false);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return entityEntry.Entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            entities.Remove(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await entities.ToListAsync().ConfigureAwait(false);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return entities.AsQueryable<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null)
        {
            IQueryable<TEntity> queryable = entities;

            if (includes != null)
            {
                queryable = includes(queryable);
            }

            return await queryable.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath)
        {
            return await entities.Include(navigationPropertyPath).ToListAsync().ConfigureAwait(false);
        }

        public virtual IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> predicate)
        {
            return entities.Where(predicate).AsNoTracking();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await entities.FindAsync(id).ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public virtual async Task UpdateAsync(object entity)
        {
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
