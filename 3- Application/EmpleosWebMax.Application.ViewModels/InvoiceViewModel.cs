using EmpleosWebMax.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace EmpleosWebMax.Application.ViewModels
{
    public class InvoiceViewModel
    {
        public InvoiceDto InvoiceDto { get; set; }
        public ModuleSequenceDto ModuleSequenceDto { get; set; }
        public List<SelectListItem> UserDto { get; set; }
        public Guid UserId { get; set; }
    }
}
