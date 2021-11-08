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
    public class ModuleSequenceService : BaseService, IModuleSequenceService
    {
        public ModuleSequenceService(IEmpleoMaxUnitOfWork unitOfWork):base(unitOfWork)
        {
        }

        public async Task<ServiceResult> AddModuleSequence(ModuleSequenceDto moduleSequenceDto)
        {
            if (moduleSequenceDto == null)
                throw new ArgumentNullException();
            
            var isExistCode = await _unitOfWork.ModuleSequencesRepository.AnyAsync(x => x.Code == moduleSequenceDto.Code);
            
            var isExistModule = await _unitOfWork.ModuleSequencesRepository.AnyAsync(x => x.Module == moduleSequenceDto.Module); 
            
            if (isExistCode)
               return ServiceResult.ResultError("Este Codigo Existe");
            
            if(isExistModule)
              return ServiceResult.ResultError("Este modulo Existe");

            var moduleSequence = new ModuleSequence
            {
                Code = moduleSequenceDto.Code,
                Sequence = moduleSequenceDto.Sequence,
                Module = moduleSequenceDto.Module,
                ModuleString = moduleSequenceDto.Module.ToString(),
                CreatedBy = moduleSequenceDto.CreatedBy,
                CreatedAt = moduleSequenceDto.CreatedAt

            };

            _unitOfWork.ModuleSequencesRepository.Add(moduleSequence);
            
            var result = await _unitOfWork.SaveChangesAsync();

            if (result < 0)
                return ServiceResult.ResultError("No se pudo guardar el registro");

            return ServiceResult.ResultSuccess();
        }

        public async Task<ServiceResultList<ModuleSequenceDto>> GetAll()
        {
            var result = await _unitOfWork.ModuleSequencesRepository.Query().Select(x => new ModuleSequenceDto 
            {
                Id = x.Id,
                Code = x.Code,
                Module = x.Module,
                Sequence = x.Sequence,
                ModuleString= x.ModuleString
            }).ToListAsync();

            return ServiceResultList<ModuleSequenceDto>.ResultSuccess(result);
        }

        public async Task<ServiceResult<ModuleSequenceDto>> GetNextSequence(ModuleSequenceEnum moduleSequenceEnum)
        {
            var result = await _unitOfWork.ModuleSequencesRepository.Query().Select(x => new ModuleSequenceDto 
            { 
              Id = x.Id,
              Code = x.Code,
              Sequence = x.Sequence + 1,
              Module = x.Module

            }).FirstOrDefaultAsync(x => x.Module == moduleSequenceEnum);

            if (result == null)
                return ServiceResult<ModuleSequenceDto>.ResultError("No se ha Encontrado la Secuencia");

            return ServiceResult<ModuleSequenceDto>.ResultSuccess(result);
        }

        public async Task<ServiceResult> UpdateSequence(ModuleSequenceDto moduleSequenceDto)
        {
            if (moduleSequenceDto == null)
                return ServiceResult.ResultError("El parametro recibido es nulo");
            var moduleSequence = await _unitOfWork.ModuleSequencesRepository.Query().FirstOrDefaultAsync(x => x.Id == moduleSequenceDto.Id);
            if (moduleSequence == null)
                return ServiceResult.ResultError("No se ha encontrado el registro");
            moduleSequence.Sequence = moduleSequenceDto.Sequence;
            _unitOfWork.ModuleSequencesRepository.Update(moduleSequence);

            var result = await _unitOfWork.SaveChangesAsync();
            if (result < 0)
                return ServiceResult.ResultError("No se ha podido guardar el registro");

            return ServiceResult.ResultSuccess();
        }
    }
}
