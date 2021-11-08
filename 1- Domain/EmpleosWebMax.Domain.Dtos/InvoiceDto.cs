using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
   public class InvoiceDto : BaseDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateFinishPlan { get; set; }
        public string Reference { get; set; }
        public string NoteToRecipient { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public Guid UserId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public ICollection<InvoiceLineDto> InvoiceLines { get; set; } = new List<InvoiceLineDto>();
    }
}
