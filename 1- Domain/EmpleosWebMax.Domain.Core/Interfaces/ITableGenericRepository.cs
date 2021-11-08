using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Core.Interfaces
{
    public interface ITableGenericRepository<T> : IRepository<T> where T : BaseEntity
    {
      
        void AddRange(T[] entity);

        Task AddRangeAsync(T[] entity);

        void Add(T entity);

        Task AddAsync(T entity);

        void Delete(T entity);

        void DeleteRange(T[] entity);


        void Update(T entity);

        void UpdateRange(T[] entity);
    }
}
