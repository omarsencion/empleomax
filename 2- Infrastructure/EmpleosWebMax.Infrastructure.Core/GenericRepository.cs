using EmpleosWebMax.Domain.Core;
using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Core.Specification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Core
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {

        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(DbContext context)
        {
            if (context == null) 
            {
                throw new Exception();
            }
            _dbSet = context.Set<TEntity>();
        }
        public bool Any(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null) 
            {
                return _dbSet.Any(where);
            }
            return _dbSet.Any();
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if(where != null) 
            {
                return await _dbSet.AnyAsync(where).ConfigureAwait(false);
            }
            return await _dbSet.AnyAsync();
        }

        public int Count(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                return _dbSet.Count(where);
            }

            return _dbSet.Count();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                return await _dbSet.CountAsync(where).ConfigureAwait(false);
            }

            return await _dbSet.CountAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                IQueryable<TEntity> query = _dbSet;
                return query.FirstOrDefault(where);
            }

            return _dbSet.FirstOrDefault();
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                IQueryable<TEntity> query = _dbSet;
                return await query.FirstOrDefaultAsync(where).ConfigureAwait(false);
            }

            return await this._dbSet.FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public TEntity GetById(params object[] keys)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetBySpecification(ISpecification<TEntity> specification)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TEntity> GetPagedElements<S>(int pageIndex, int pageCount, Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>includes = null)
        {
            var query = this._dbSet.AsNoTracking();
            if (includes != null)
            {
                query = includes(query);
            }

            return query;
        }

        //public Task<IQueryable<TEntity>> QueryAsync(string[] includes = null)
        //{
        //    //var query = this._dbSet.AsNoTracking();
        //    //if (includes != null && includes.Any())
        //    //{
        //    //    query = includes.Aggregate(query, (current, inc) => current.Include(inc));
        //    //}

        //    //return query;
        //}

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                IQueryable<TEntity> query = _dbSet;
                return query.SingleOrDefault(where);
            }

            return _dbSet.SingleOrDefault();
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> where = null)
        {
            if (where != null)
            {
                IQueryable<TEntity> query = _dbSet;
                return await query.SingleOrDefaultAsync(where).ConfigureAwait(false);
            }

            return await _dbSet.SingleOrDefaultAsync().ConfigureAwait(false);
        }
    }
}
