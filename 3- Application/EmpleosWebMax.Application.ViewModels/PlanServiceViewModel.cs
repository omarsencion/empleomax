using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Application.ViewModels
{
    public class PlanServiceViewModel
    {
        public PlanServiceDto PlanService { get; set; }
        public string ServiceId { get; set; }
        public Category Category { get; set; }

        public List<SelectListItem> Services { get; set; } = new List<SelectListItem>();
    }
}
