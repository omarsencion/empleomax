using EmpleosWebMax.Domain.Core.Specification;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Core.Interfaces
{
    public interface IRepository<TEntity>: IDisposable where TEntity: BaseEntity
    {
        bool Any(Expression<Func<TEntity, bool>> where = null);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where = null);
        int Count(Expression<Func<TEntity, bool>> where = null);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where = null);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null);
        IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includes = null);
       // Task<IQueryable<TEntity>> QueryAsync(string[] includes = null);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where = null);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where = null);
        TEntity GetById(params object[] keys);
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetBySpecification(ISpecification<TEntity> specification);
        IEnumerable<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, bool ascending);

    }
}
