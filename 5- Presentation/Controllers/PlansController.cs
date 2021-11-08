using EmpleosWebMax.Application.ViewModels;
using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.UI.Web.Controllers
{
    public class PlansController : Controller
    {
        private IPlanService _service;
        private IModuleSequenceService _moduleSequenceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public PlansController(IPlanService service, IModuleSequenceService moduleSequenceService, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _moduleSequenceService = moduleSequenceService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetPlans();
            return View(result.Data);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> AddPlan()
        {
            var codeSequence = await _moduleSequenceService.GetNextSequence(Common.Enum.ModuleSequenceEnum.Plan);
            if (codeSequence.ResultKind == ResultKind.Error)
                ViewBag.Message = codeSequence.Error;
            var planViewModel = new PlanViewModel
            {
                PlanDto = new PlanDto
                {
                    Code = codeSequence.Data.CodeSequences
                },
                ModuleSequenceDto = codeSequence.Data
            };
            
            return View(planViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlan(PlanViewModel planViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);

                planViewModel.PlanDto.CreatedBy = $"{user.FirstName} {user.LastName}";
                planViewModel.PlanDto.CreatedAt = DateTime.Now;

               var result= await _service.AddPlan(planViewModel.PlanDto);
                if (result.ResultKind == ResultKind.Success)
                {
                    var sequenceDto = new ModuleSequenceDto
                    {
                        Id = planViewModel.ModuleSequenceDto.Id,
                        Module = planViewModel.ModuleSequenceDto.Module,
                        Sequence = planViewModel.ModuleSequenceDto.Sequence
                    };
                    var resultSequence = await _moduleSequenceService.UpdateSequence(sequenceDto);
                   
                    return RedirectToAction("EditPlan", new {id = result.Data.Id  });
                }
                ViewBag.Message = result.Error;
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditPlan(Guid id)
        {
           
            if (id == null)
            {
                return NotFound();
            }
           
            var result = await _service.GetPlanById(id);

            return View(result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> EditPlan(PlanDto plan)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                plan.LastModifiedBy = $"{user.FirstName} {user.LastName}";
                plan.LastModifiedAt = DateTime.Now;
               var result = await _service.UpdatePlan(plan);
                if (result.ResultKind == ResultKind.Success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Message = result.Error;
            }

            return View();
        }
    }
}
