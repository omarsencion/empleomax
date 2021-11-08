using EmpleosWebMax.Common.Enum;
using EmpleosWebMax.Domain.Core.Interfaces;
using EmpleosWebMax.Domain.Core.Result;
using EmpleosWebMax.Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EmpleosWebMax.Infrastructure.Interface.InterfaceService
{
    public interface ITaxReceiptService: IBaseService
    {
        Task<ServiceResultList<TaxReceiptDto>> GetAll();
        Task<ServiceResult<TaxReceiptDto>> GetById(Guid id);
        Task<ServiceResult> AddTaxRecipt(TaxReceiptDto taxReceiptDto);
        Task<ServiceResult<TaxReceiptDto>> GetNextSequence(Category category);
        Task<ServiceResult> UpdateTaxRecipt(TaxReceiptDto taxReceiptDto);
    }
}
