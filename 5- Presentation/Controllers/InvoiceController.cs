using EmpleosWebMax.Application.ViewModels;
using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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


        [HttpGet]
        public async Task<IActionResult> Edit(Guid? edit)
        {
            var invoiceViewModelS = new InvoiceViewModel
            {

                InvoiceNumber = "a000121410d",
                Date = DateTime.Now,
                DateFinishPlan = DateTime.Now,
                Discount = 00,
                IsPaid = false,
                Reference = "Ejemplo de referencia",
                Subtotal = 250,
                TaxAmount = 01,
                NoteToRecipient = "A",
                UserId = Guid.NewGuid(),
                SubscriptionType = SubscriptionType.Montly,
                Total = 044,
                SubscriptionId = Guid.NewGuid(),
                UserEmail = "a@a.com",
                UserName = "pablo13sanchez"



            };



            return View(invoiceViewModelS);

        }



        [HttpGet]
        public async Task<IActionResult> Detail(Guid? edit)
        {
            var invoiceListLine = new List<InvoiceLineDto>();



            for (int i = 0; i < 20; i++)
            {

                var InvoiceLineLine = new InvoiceLineDto
                {
                    Amount = 42,
                    Description = "Esto es una prueba",
                    InvoiceId = Guid.NewGuid(),
                    PlanName = $"Plan {i}",
                    TaxAmount = i,
                    Quantity = 2,
                    PlanId = Guid.NewGuid(),
                    Price = 01,
                    ServiceName = "Service Name",
                    TotalAmount = 05,
                    Id = Guid.NewGuid(),
                    ServiceId = Guid.NewGuid()
                };
                invoiceListLine.Add(InvoiceLineLine);

            }

       

            var invoiceViewModelS = new InvoiceViewModel
            {

                InvoiceNumber = "a000121410d",
                Date = DateTime.Now,
                DateFinishPlan = DateTime.Now,
                Discount = 00,
                IsPaid = false,
                Reference = "Ejemplo de referencia",
                Subtotal = 250,
                TaxAmount = 01,
                NoteToRecipient = "A",
                UserId = Guid.NewGuid(),
                SubscriptionType = SubscriptionType.Montly,
                Total = 044,
                SubscriptionId = Guid.NewGuid(),
                UserEmail = "a@a.com",
                UserName = "pablo13sanchez",
                InvoiceLinesWithOutPaging = invoiceListLine


            };



            return View(invoiceViewModelS);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(InvoiceViewModel edit)
        {

            return View();
        }

        [HttpGet]
        [HttpPost]

        public async Task<IActionResult> Index(int? page)
        {

            var invoiceListLine = new List<InvoiceLineDto>();



            for (int i = 0; i < 20; i++)
            {

                var InvoiceLineLine = new InvoiceLineDto
                {
                    Amount = 42,
                    Description = "Esto es una prueba",
                    InvoiceId = Guid.NewGuid(),
                    PlanName = $"Plan {i}",
                    TaxAmount = i,
                    Quantity = 2,
                    PlanId = Guid.NewGuid(),
                    Price = 01,
                    ServiceName = "Service Name",
                    TotalAmount = 05,
                    Id = Guid.NewGuid(),
                    ServiceId = Guid.NewGuid()
                };
                invoiceListLine.Add(InvoiceLineLine);

            }


            var resultadodeLista = invoiceListLine.ToPagedList(page ?? 1, 5);

            var invoiceViewModelS = new InvoiceViewModel
            {

                InvoiceNumber = "a000121410d",
                Date = DateTime.Now,
                DateFinishPlan = DateTime.Now,
                Discount = 00,
                IsPaid = false,
                Reference = "Ejemplo de referencia",
                Subtotal = 250,
                TaxAmount = 01,
                NoteToRecipient = "A",
                UserId = Guid.NewGuid(),
                SubscriptionType = SubscriptionType.Montly,
                Total = 044,
                InvoiceLines = resultadodeLista,
                SubscriptionId = Guid.NewGuid(),
                UserEmail = "a@a.com",
                UserName = "pablo13sanchez"



            };



            return View(invoiceViewModelS);
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

                Text = $"{x.FirstName} {x.LastName}",
                Value = x.Id
            }).ToListAsync();

            var invoiceViewModel = new InvoiceViewModel
            {/*
                InvoiceDto = new InvoiceDto
                {
                    InvoiceNumber = codeSequence.Data.CodeSequences
                },
                ModuleSequenceDto = codeSequence.Data,
                UserDto = userList*/
            };

            return View(invoiceViewModel);
        }
    }



}
