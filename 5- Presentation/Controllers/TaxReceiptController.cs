using EmpleosWebMax.Application.ViewModels;
using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmpleosWebMax.UI.Web.Controllers
{
    [Authorize]
    public class TaxReceiptController : Controller
    {
        private readonly ITaxReceiptService _taxReceiptService;
        private IModuleSequenceService _moduleSequenceService;
        public TaxReceiptController(ITaxReceiptService taxReceiptService, IModuleSequenceService moduleSequenceService)
        {
            _taxReceiptService = taxReceiptService;
            _moduleSequenceService = moduleSequenceService;
        }
        
        public async Task<IActionResult> Index()
        {
            var result = await _taxReceiptService.GetAll();
            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> AddTaxReceipt() 
        {
            var codeSequence = await _moduleSequenceService.GetNextSequence(Common.Enum.ModuleSequenceEnum.TaxReceipt);
            if (codeSequence.ResultKind == Common.Enum.ResultKind.Error) 
            {
                ViewBag.Message = codeSequence.Error;
                return RedirectToAction("Index");
            }
            var taxReceiptViewModel = new TaxReceiptViewModel
            {
                TaxReceiptDto = new TaxReceiptDto
                {
                    Code = codeSequence.Data.CodeSequences
                },
                ModuleSequenceDto = codeSequence.Data
            };

            return View(taxReceiptViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTaxReceipt(TaxReceiptViewModel taxReceiptViewModel) 
        {
            if (ModelState.IsValid) 
            {
                var result = await _taxReceiptService.AddTaxRecipt(taxReceiptViewModel.TaxReceiptDto);
                if (result.ResultKind == ResultKind.Success) 
                {
                    var sequenceDto = new ModuleSequenceDto
                    {
                        Id = taxReceiptViewModel.ModuleSequenceDto.Id,
                        Module = taxReceiptViewModel.ModuleSequenceDto.Module,
                        Sequence = taxReceiptViewModel.ModuleSequenceDto.Sequence
                    };
                    var resultSequence = await _moduleSequenceService.UpdateSequence(sequenceDto);
                    return RedirectToAction("Index");
                }
                ViewBag.Error = result.Error;
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> EditTaxReceipt(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = await _taxReceiptService.GetById(id);
            if (result.ResultKind == Common.Enum.ResultKind.Success)
            {
                return View(result.Data);
            }
            ViewBag.Message = result.Error;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditTaxReceipt(TaxReceiptDto taxReceiptDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _taxReceiptService.UpdateTaxRecipt(taxReceiptDto);

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
