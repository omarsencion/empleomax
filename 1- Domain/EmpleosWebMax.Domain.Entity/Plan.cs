using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class Plan : AuditableEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public bool IsDefaultPlan { get; set; }
        public int JobVacancyNumber { get; set; }
        public int PriorityJobNumber { get; set; }
        public int CanditateMessageNumber { get; set; }
        public bool IsCanditateSearch { get; set; }
        public int PriceOfJobVacancyBonus { get; set; }
        public State State { get; set; }

        public virtual ICollection<PlanService> PlanServices { get; set; } = new List<PlanService>();
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
    }
}
