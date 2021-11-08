using EmpleosWebMax.Application.ViewModels;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.UI.Web.Controllers
{
    public class InvoiceController : Controller
    {
        private ApplicationDbContext _applicationDbContext;
        private IInvoiceServices _InvoiceService; 
        private IModuleSequenceService _moduleSequenceService;
        public InvoiceController(ApplicationDbContext applicationDbContext, IModuleSequenceService moduleSequenceService, IInvoiceServices invoiceServices)
        {
            _applicationDbContext = applicationDbContext;
            _moduleSequenceService = moduleSequenceService;
            _InvoiceService = invoiceServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddInvoice() 
        {
            var codeSequence = await _moduleSequenceService.GetNextSequence(Common.Enum.ModuleSequenceEnum.Invoice);

            if (codeSequence.ResultKind == Common.Enum.ResultKind.Error)
            {

                ViewBag.Message = codeSequence.Error;
                return RedirectToAction("Index");
            }
            
            var userList = await _applicationDbContext.Users.Where(x => x.TypeUser == 255485).Select(x => new SelectListItem
            {
                
                Text= $"{x.FirstName} {x.LastName}",
                Value = x.Id
            }).ToListAsync();

            var invoiceViewModel = new InvoiceViewModel
            {
                InvoiceDto = new InvoiceDto
                {
                    InvoiceNumber = codeSequence.Data.CodeSequences
                },
                ModuleSequenceDto = codeSequence.Data,
                UserDto = userList
            };

            return View(invoiceViewModel);
        }
    }

}
