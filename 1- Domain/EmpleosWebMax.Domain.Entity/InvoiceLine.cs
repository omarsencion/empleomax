using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class InvoiceLine : BaseEntity
    {
        
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public Guid ServiceId { get; set; }
        public Service Service { get; set; }
        public Guid PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
