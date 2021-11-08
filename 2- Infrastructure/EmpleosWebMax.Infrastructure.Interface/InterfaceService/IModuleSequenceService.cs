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
    public interface IModuleSequenceService: IBaseService
    {
        Task<ServiceResultList<ModuleSequenceDto>> GetAll();
        Task<ServiceResult> AddModuleSequence(ModuleSequenceDto moduleSequenceDto);
        Task<ServiceResult<ModuleSequenceDto>> GetNextSequence(ModuleSequenceEnum moduleSequenceEnum);
        Task<ServiceResult> UpdateSequence(ModuleSequenceDto moduleSequenceDto);
    }
}
