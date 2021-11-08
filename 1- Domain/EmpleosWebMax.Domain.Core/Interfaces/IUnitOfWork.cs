using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Domain.Core.Interfaces
{
   public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        Task BeginTransactionAsync();

        void Commit();
        Task CommitAsync();
        void CommitAndRefreshChange();
        Task<int> CommitAndRefreshChangeAsync();
        void RollbackChanges();
        Task RollbackTransactionAsync();
        void Refresh(object entity);
        int SaveChanges();
        Task<int> SaveChangesAsync();

    }
}
