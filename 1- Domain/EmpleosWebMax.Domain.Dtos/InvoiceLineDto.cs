using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
    public class InvoiceLineDto: BaseDto
    {
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid InvoiceId { get; set; }
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public Guid PlanId { get; set; }
        public string PlanName { get; set; }
    }
}
