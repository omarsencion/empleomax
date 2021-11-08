using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class Service : AuditableEntity
    {
        public string Code { get; set; }
        public Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public State State { get; set; }

        public virtual ICollection<PlanService> PlanServices { get; set; } = new List<PlanService>();
        public virtual ICollection<InvoiceLine> InvoiceLine { get; set; } = new List<InvoiceLine>();

    }
}
