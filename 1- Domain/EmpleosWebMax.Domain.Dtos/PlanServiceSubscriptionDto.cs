using EmpleosWebMax.Domain.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Domain.Dtos
{
    public class PlanServiceSubscriptionDto: BaseDto
    {
        public Guid PlanId { get; set; }
        public string PlanName { get; set; }
        public decimal PlanPrice { get; set; }
        public decimal PlanPriceAnnual => PlanPrice * 10;
        public bool IsDefaultPlan { get; set; }

        public List<ServiceDto> ServicesPlan { get; set; } = new List<ServiceDto>();
    }
}
