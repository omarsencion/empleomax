using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Interface.InterfaceService
{
    public interface IInvoiceServices: IBaseService
    {
        Task<ServiceResultList<InvoiceDto>> GetAll();
        Task<ServiceResult> AddInvoice(InvoiceDto invoiceDto);
        Task<ServiceResult> UpdateInvoice(InvoiceDto invoiceDto);
        Task<ServiceResult<InvoiceDto>> GetInvoiceById(Guid invoiceId);
    }
}
