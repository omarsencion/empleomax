using EmpleosWebMax.Domain.Core;
using EmpleosWebMax.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Core
{
    public class TableGenericRepository<TEntity> : GenericRepository<TEntity>, ITableGenericRepository<TEntity> where TEntity : BaseEntity
    {

        public TableGenericRepository(DbContext context) : base(context)
        {
        }
        public void Add(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public async Task AddAsync(TEntity entity)
        {
            await this._dbSet.AddAsync(entity).ConfigureAwait(false);
        }

        public void AddRange(TEntity[] entity)
        {
            _dbSet.AddRange(entity);
        }

        public async Task AddRangeAsync(TEntity[] entity)
        {
            await _dbSet.AddRangeAsync(entity).ConfigureAwait(false);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity); 
        }

        public void DeleteRange(TEntity[] entity)
        {
            _dbSet.RemoveRange(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(TEntity[] entity)
        {
            _dbSet.UpdateRange(entity);
        }
    }
}
