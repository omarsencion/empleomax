using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Dtos;
using EmpleosWebMax.Domain.Entity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace EmpleosWebMax.Application.ViewModels
{
    public class InvoiceViewModel
    {
        //public InvoiceDto InvoiceDto { get; set; }
        //public ModuleSequenceDto ModuleSequenceDto { get; set; }
        //public List<SelectListItem> UserDto { get; set; }
        //public Guid UserId { get; set; }


        public string InvoiceNumber { get; set; }
        public DateTime Date { get; set; }
        public DateTime DateFinishPlan { get; set; }
        public string Reference { get; set; }
        public string NoteToRecipient { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool IsPaid { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public UserDto MyProperty { get; set; }
        public Guid SubscriptionId { get; set; }
        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }
        public string SubscriptionName { get; set; }
        public SubscriptionType SubscriptionType { get; set; }
        public ICollection<InvoiceLineDto> InvoiceLinesWithOutPaging { get; set; } = new List<InvoiceLineDto>();
        public IPagedList<InvoiceLineDto> InvoiceLines { get; set; }


    }
}
