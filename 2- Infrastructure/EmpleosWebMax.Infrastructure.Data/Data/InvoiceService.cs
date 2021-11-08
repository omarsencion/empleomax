using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using EmpleosWebMax.Infrastructure.Core;
using EmpleosWebMax.Infrastructure.Interface.InterfaceService;
using EmpleosWebMax.Infrastructure.Interface.Module;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Data.Data
{
    public class InvoiceService : BaseService, IInvoiceServices
    {
        private ISubscriptionService _subscriptionService;
        public InvoiceService(IEmpleoMaxUnitOfWork unitOfWork, ISubscriptionService subscriptionService) : base(unitOfWork)
        {
            _subscriptionService = subscriptionService;
        }

        public async Task<ServiceResult> AddInvoice(InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
                return ServiceResult.ResultError("Invoice is null");

            var planId = Guid.Empty;
            var PlanName = string.Empty;

            var invoice = new Invoice
            {
                ApplicationUserId = invoiceDto.UserId,
                Date = invoiceDto.Date,
                Discount = invoiceDto.Discount,
                InvoiceNumber = invoiceDto.InvoiceNumber,
                Reference = invoiceDto.Reference,
                IsPaid = invoiceDto.IsPaid,
                NoteToRecipient = invoiceDto.NoteToRecipient,
                Subtotal = invoiceDto.Subtotal,
                TaxAmount = invoiceDto.TaxAmount,
                Total = invoiceDto.Total
            };

            invoiceDto.InvoiceLines.ToList().ForEach(x =>
            {
                if (x.PlanId != Guid.Empty)
                {
                    planId = x.PlanId;
                    PlanName = x.PlanName;
                }
                invoice.InvoiceLines.Add(new InvoiceLine
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    PlanId = x.PlanId,
                    Price = x.Price,
                    TaxAmount = x.TaxAmount,
                    Quantity = x.Quantity,
                    TotalAmount = x.TotalAmount,
                    ServiceId = x.ServiceId,

                });
            });

            _unitOfWork.InvoiceRepository.Add(invoice);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 1)
            {
                if (invoice.IsPaid)
                {
                    var subscription = new SubscriptionDto
                    {
                        ApplicationUserId = invoice.ApplicationUserId.ToString(),
                        PlanId = planId,
                        PlanName = PlanName,
                        PlanPrice = invoice.Total,
                        DateFrom = invoice.Date,
                        DateTo = invoiceDto.DateFinishPlan,
                        CreatedDate = DateTime.UtcNow,
                        TotalPrice = invoice.Total,
                        SubscriptionType = invoiceDto.SubscriptionType,
                        IsPaid = true,
                        IsActive = true
                    };
                    var resultSubscription = await _subscriptionService.AddSubscription(subscription);
                    if (resultSubscription.ResultKind == Common.Enum.ResultKind.Error)
                    {
                        _unitOfWork.InvoiceRepository.Delete(invoice);
                        var resultDelete = await _unitOfWork.SaveChangesAsync();
                        if (resultDelete == 1)
                            return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                        else
                            return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                    }

                    invoice.SubscriptionId = resultSubscription.Data.Id;

                    _unitOfWork.InvoiceRepository.Update(invoice);

                    var resultUpdate = await _unitOfWork.SaveChangesAsync();

                    if (resultUpdate == 1)
                        return ServiceResult.ResultSuccess();
                    else
                        return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                }
            }

            return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura"); 
        }

        public async Task<ServiceResultList<InvoiceDto>> GetAll()
        {
            var result = await _unitOfWork.InvoiceRepository.Query().Select(x => new InvoiceDto { 
             Id = x.Id,
             InvoiceNumber = x.InvoiceNumber,
             Date = x.Date,
             Discount = x.Discount,
             NoteToRecipient = x.NoteToRecipient,
             Reference = x.Reference,
             Total = x.Total,
             IsPaid = x.IsPaid,
             Subtotal = x.Subtotal,
             TaxAmount = x.TaxAmount
            }).ToListAsync();

            return ServiceResultList<InvoiceDto>.ResultSuccess(result);
        }

        public Task<ServiceResult<InvoiceDto>> GetInvoiceById(Guid invoiceId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateInvoice(InvoiceDto invoiceDto)
        {
            if (invoiceDto == null)
                return ServiceResult.ResultError("Invoice is null");

            var invoice = await _unitOfWork.InvoiceRepository.FirstOrDefaultAsync(x => x.Id == invoiceDto.Id);

            if (invoice == null)
                return ServiceResult.ResultError("No fue Encontrado la factura con este Id");

            invoice.Reference = invoiceDto.Reference;
            invoice.Subtotal = invoiceDto.Subtotal;
            invoice.TaxAmount = invoiceDto.TaxAmount;
            invoice.Total = invoiceDto.Total;
            invoice.ApplicationUserId = invoiceDto.UserId;
            invoice.Discount = invoiceDto.Discount;
            invoice.NoteToRecipient = invoiceDto.NoteToRecipient;
            invoice.IsPaid = invoiceDto.IsPaid;

            _unitOfWork.InvoiceRepository.Update(invoice);
            var result = await _unitOfWork.SaveChangesAsync();
            if (result == 1)
            {
                if (invoice.IsPaid)
                {
                    var invoiceLine = await _unitOfWork.InvoiceLineRepository.Query(t => t.Include(r => r.Plan)).FirstOrDefaultAsync(x => x.InvoiceId == invoice.Id && x.PlanId != Guid.Empty);
                    var subscription = new SubscriptionDto
                    {
                        ApplicationUserId = invoice.ApplicationUserId.ToString(),
                        PlanId = invoiceLine.PlanId,
                        PlanName = invoiceLine.Plan.Name,
                        PlanPrice = invoice.Total,
                        DateFrom = invoice.Date,
                        DateTo = invoiceDto.DateFinishPlan,
                        CreatedDate = DateTime.UtcNow,
                        TotalPrice = invoice.Total,
                        SubscriptionType = invoiceDto.SubscriptionType,
                        IsPaid = true,
                        IsActive = true
                    };
                    var resultSubscription = await _subscriptionService.AddSubscription(subscription);
                    if (resultSubscription.ResultKind == Common.Enum.ResultKind.Error)
                    {
                        invoice.IsPaid = false;
                        _unitOfWork.InvoiceRepository.Update(invoice);
                        var resultDelete = await _unitOfWork.SaveChangesAsync();
                        if (resultDelete == 1)
                            return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                        else
                            return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                    }

                    invoice.SubscriptionId = resultSubscription.Data.Id;

                    _unitOfWork.InvoiceRepository.Update(invoice);

                    var resultUpdate = await _unitOfWork.SaveChangesAsync();

                    if (resultUpdate == 1)
                        return ServiceResult.ResultSuccess();
                    else
                        return ServiceResult.ResultError("Ocurrio un error durante la creacion de la factura");
                }
            }
            return ServiceResult.ResultError("Ocurrio un error durante la Modificacion de la factura");
        }
    }
}