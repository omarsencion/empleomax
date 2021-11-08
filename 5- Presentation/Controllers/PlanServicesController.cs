using EmpleosWebMax.Application.ViewModels;
using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.AspNet.Helpers;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.UI.Web.Controllers
{
    [ViewComponent(Name ="PlanServicesComponent")]
    public class PlanServicesController : Controller
    {
        private IPlanServiceService _planServiceServices;
        private IServiceService _serviceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PlanServicesController(IPlanServiceService planServiceServices, IServiceService serviceService,  UserManager<ApplicationUser> userManager)
        {
            _planServiceServices = planServiceServices;
            _serviceService = serviceService;
            _userManager = userManager;
        }

        public  IViewComponentResult Invoke(Guid planId, Category category)
        {
            ViewBag.planId = planId;
            ViewBag.Category = category;
            var result =  _planServiceServices.GetByPlanId(planId).Result.Data;
            
            return new ViewViewComponentResult()
            {
                ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary<IEnumerable<PlanServiceDto>>(ViewData, result)
            };
        }

        [HttpGet]
        public async Task<IActionResult> AddPlanService(Guid planId,Category category) 
        {
            var resultService = await _serviceService.GetServicesByCategory(category);
            var serviceByCategory = resultService.Data.ToList();
            serviceByCategory.Add(new ServiceDto
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Name = "Seleccione Un Servicio"

            });
            var plaServiceViewModel = new PlanServiceViewModel
            {
                Category = category,
                PlanService = new PlanServiceDto
                {
                    PlanId = planId
                },
                Services = serviceByCategory.Select(x => new SelectListItem { Value= x.Id.ToString(), Text=x.Name, Selected= true }).ToList()
            };
            
            return View(plaServiceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlanService(PlanServiceViewModel planServiceViewModel)
        {
            if (ModelState.IsValid) 
            {
                if (planServiceViewModel == null)
                    return Json(new { isValid = false });
                if (planServiceViewModel.ServiceId.Equals("00000000-0000-0000-0000-000000000000"))
                    return Json(new {isValid = false });
                    
                var user = await _userManager.GetUserAsync(User);
                var planServiceDto = new PlanServiceDto
                    {
                        PlanId = planServiceViewModel.PlanService.PlanId,
                        ServicesId = Guid.Parse(planServiceViewModel.ServiceId),
                        CreatedAt = DateTime.Now,
                        CreatedBy = $"{user.FirstName} {user.LastName}"
                    };
                
                var result=  await _planServiceServices.AddPlanService(planServiceDto);
                if (result.ResultKind == ResultKind.Success)
                {
                    Invoke(planServiceDto.Id, planServiceViewModel.Category);
                    return Json(new { isValid = true });
                }
                return Json(new { isValid = false, message= result.Error });
            }

            return Json(new { isValid = false, data=planServiceViewModel.PlanService.Id });
        }
    }
}
