using EmpleosWebMax.Common.Enum;
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
    public class TaxReceiptService : BaseService, ITaxReceiptService
    {
        public TaxReceiptService(IEmpleoMaxUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<ServiceResult> AddTaxRecipt(TaxReceiptDto taxReceiptDto)
        {
            if (taxReceiptDto == null)
                return ServiceResult.ResultError("El parametro es nulo");
            var addTaxReceipt = new TaxReceipt 
            {
                Code = taxReceiptDto.Code,
                IsCanditate = taxReceiptDto.IsCanditate,
                IsCompany = taxReceiptDto.IsCompany,
                IsInternationalCompany = taxReceiptDto.IsInternationalCompany,
                Name = taxReceiptDto.Name,
                Prefix = taxReceiptDto.Prefix,
                SequenceFrom = taxReceiptDto.SequenceFrom,
                SequenceTo = taxReceiptDto.SequenceTo,
                SequenceActual = taxReceiptDto.SequenceActual
            };
            _unitOfWork.TaxReceiptRepository.Add(addTaxReceipt);
            
            var result = await _unitOfWork.SaveChangesAsync();
            
            if (result == 0)
                return ServiceResult.ResultError("No se ha podido guardar el registro");

            return ServiceResult.ResultSuccess();
        }

        public async Task<ServiceResultList<TaxReceiptDto>> GetAll()
        {
            var result = await _unitOfWork.TaxReceiptRepository.Query().Select(x => new TaxReceiptDto { 
              Id = x.Id,
              Code = x.Code,
              Name = x.Name,
              Prefix = x.Prefix,
              SequenceActual = x.SequenceActual,
              SequenceFrom = x.SequenceFrom,
              SequenceTo = x.SequenceTo,
              IsCanditate = x.IsCanditate,
              IsCompany = x.IsCompany,
              IsInternationalCompany = x.IsInternationalCompany
            }).ToListAsync();

            return ServiceResultList<TaxReceiptDto>.ResultSuccess(result);
        }

        public async Task<ServiceResult<TaxReceiptDto>> GetById(Guid id)
        {
            if (id == null)
                return null;
            var result = await _unitOfWork.TaxReceiptRepository.FirstOrDefaultAsync(x => x.Id == id);

            if (result != null) 
            {
                return ServiceResult<TaxReceiptDto>.ResultSuccess(new TaxReceiptDto 
                {
                    Id = result.Id,
                    Code = result.Code,
                    Name = result.Name,
                    Prefix = result.Prefix,
                    SequenceFrom = result.SequenceFrom,
                    SequenceTo = result.SequenceTo,
                    SequenceActual = result.SequenceActual,
                    IsCanditate = result.IsCanditate,
                    IsCompany = result.IsCompany,
                    IsInternationalCompany = result.IsInternationalCompany,
                });
            }
            return ServiceResult<TaxReceiptDto>.ResultError("No Existe ningun Comprobante fiscal con este Id");
        }

        public Task<ServiceResult<TaxReceiptDto>> GetNextSequence(Category category)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResult> UpdateTaxRecipt(TaxReceiptDto taxReceiptDto)
        {
            if (taxReceiptDto == null)
                return ServiceResult.ResultError("El parametro es nulo");
            
            var result = await _unitOfWork.TaxReceiptRepository.Query().FirstOrDefaultAsync(x => x.Id == taxReceiptDto.Id);
            
            result.Code = taxReceiptDto.Code;
            result.Name = taxReceiptDto.Name;
            result.Prefix = taxReceiptDto.Prefix;
            result.SequenceFrom = taxReceiptDto.SequenceFrom;
            result.SequenceTo = taxReceiptDto.SequenceTo;
            result.SequenceActual = taxReceiptDto.SequenceActual;
            result.IsCanditate = taxReceiptDto.IsCanditate;
            result.IsCompany = taxReceiptDto.IsCompany;
            result.IsInternationalCompany = taxReceiptDto.IsInternationalCompany;

            _unitOfWork.TaxReceiptRepository.Update(result);

            if(await _unitOfWork.SaveChangesAsync() == 0)
                return ServiceResult.ResultError("No se ha podido guardar el registro");
            
            return ServiceResult.ResultSuccess();
        }
    }
}
