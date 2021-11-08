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
    [Authorize]
    public class ModuleSequenceController : Controller
    {
        private readonly IModuleSequenceService _service;
        private readonly UserManager<ApplicationUser> _userManager;
        public ModuleSequenceController(IModuleSequenceService service, UserManager<ApplicationUser> userManager)
        {
           _service = service;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAll();
            
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> AddModuleSequence()
        {
            await Task.FromResult(0);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddModuleSequence(ModuleSequenceDto moduleSequence)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                
                moduleSequence.CreatedAt = DateTime.Now;
                moduleSequence.CreatedBy = $"{user.FirstName} {user.LastName}";

                var result = await _service.AddModuleSequence(moduleSequence);
                
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
