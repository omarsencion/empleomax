using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Core.Interfaces
{
    public interface IContextBase: IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity;
        EntityEntry<T> Entry<T>(T entity) where T : BaseEntity; 
    }
}
