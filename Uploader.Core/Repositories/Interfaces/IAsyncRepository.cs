using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Uploader.Infrastructure.Data.Entities.Base;

namespace Uploader.Core.Repositories.Interfaces
{
    public interface IAsyncRepository<TEntity> where TEntity : Entity
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        IQueryable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetAllAsync<TProperty>(Expression<Func<TEntity, TProperty>> navigationPropertyPath);
        Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
        Task<TEntity> GetByIdAsync(int id);
        IQueryable<TEntity> GetByWhere(Expression<Func<TEntity, bool>> predicate);
        Task UpdateAsync(TEntity entity);
        Task UpdateAsync(object entity);
        Task SaveChangesAsync();
    }
}
