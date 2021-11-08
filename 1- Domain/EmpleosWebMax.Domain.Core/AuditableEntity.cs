using EmpleosWebMax.Domain.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Core
{
    public abstract class AuditableEntity: BaseEntity, IAuditableEntity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedAt { get; set; }
    }
}
