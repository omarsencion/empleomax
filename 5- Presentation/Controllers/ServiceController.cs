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

    // [Route("Admin/{controller}")]
    [Authorize]
    public class ServiceController : Controller
    {
        private IServiceService _service;
        private IModuleSequenceService _moduleSequenceService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ServiceController(IServiceService service, IModuleSequenceService moduleSequenceService, UserManager<ApplicationUser> userManager)   
        {
            _service = service;
            _moduleSequenceService = moduleSequenceService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetServices();
            return View(result.Data);
        }
        [HttpGet]
        
        public async Task<IActionResult> AddService() 
        {
            var codeSequence = await _moduleSequenceService.GetNextSequence(Common.Enum.ModuleSequenceEnum.Service);

            if (codeSequence.ResultKind == Common.Enum.ResultKind.Error)
            {

                ViewBag.Message = codeSequence.Error;
                return RedirectToAction("Index");
            }
            var serviceViewModel = new ServiceViewModel
            {
                ServiceDto = new ServiceDto
                {
                    Code = codeSequence.Data.CodeSequences
                },
                ModuleSequenceDto = codeSequence.Data
            };
            return View(serviceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddService(ServiceViewModel serviceViewModel) 
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);

                serviceViewModel.ServiceDto.CreatedBy = $"{user.FirstName} {user.LastName}";
                serviceViewModel.ServiceDto.CreatedAt = DateTime.Now;

                var result = await _service.AddService(serviceViewModel.ServiceDto);
                if (result.ResultKind == ResultKind.Success)
                {
                    var sequenceDto = new ModuleSequenceDto
                    {
                        Id = serviceViewModel.ModuleSequenceDto.Id,
                        Module = serviceViewModel.ModuleSequenceDto.Module,
                        Sequence = serviceViewModel.ModuleSequenceDto.Sequence
                    };
                    var resultSequence = await _moduleSequenceService.UpdateSequence(sequenceDto);
                    return RedirectToAction("Index");
                }
                ViewBag.Message = result.Error;
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> EditService(Guid id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            var result = await _service.GetServiceById(id);
            if (result.ResultKind == Common.Enum.ResultKind.Success)
            {
                return View(result.Data);
            }
            ViewBag.Message = result.Error;
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditService(ServiceDto service) 
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.GetUserAsync(User);

                service.LastModifiedBy = $"{user.FirstName} {user.LastName}";
                service.LastModifiedAt = DateTime.Now;
                
                var result = await _service.UpdateService(service);
                
                if (result.ResultKind == ResultKind.Success)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Message = result.Error;
            }

            return View();
        }
        public async Task<IActionResult> Delete(Guid serviceId) 
        {
           // var result = _service.
           return RedirectToAction("Index");
        }
    }
}
