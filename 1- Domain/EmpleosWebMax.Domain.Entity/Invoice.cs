using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Entity
{
    public class Invoice : BaseEntity
    {
        public string  InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public string Reference { get; set; }
        public string NoteToRecipient { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }

        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public Guid SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }

        public virtual ICollection<InvoiceLine> InvoiceLines { get; set; } = new List<InvoiceLine>();
        
    }
}
