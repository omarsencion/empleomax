using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class PlanService : AuditableEntity
    {
        public Guid PlanId { get; set; }
        public virtual Plan Plans { get; set; }
        public Guid ServiceId { get; set; }
        public virtual Service Services { get; set; }
    }
}
