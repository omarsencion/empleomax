using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
    public class SubscriptionDto:BaseDto
    {
        public SubscriptionType SubscriptionType { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public Guid PlanId { get; set; }
        public string PlanName { get; set; }
        public string ApplicationUserId { get; set; }

        public decimal PlanPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string Payment { get; set; }
        public bool IsPaid { get; set; }
        public bool IsActive { get; set; }
        public bool IsAutoRenewel { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
