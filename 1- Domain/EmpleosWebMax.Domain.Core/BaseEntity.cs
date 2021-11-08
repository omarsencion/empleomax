using EmpleosWebMax.Domain.Core.Interfaces;
using System;

namespace EmpleosWebMax.Domain.Core
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

    }
}
