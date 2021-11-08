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
   public interface IPlanService : IBaseService
    {
        Task<ServiceResultList<PlanDto>> GetPlans();
        Task<ServiceResult<PlanDto>> GetPlanDefaultByCategory(Category category);
        Task<ServiceResult<PlanDto>> GetPlanById(Guid id);
        Task<ServiceResult<PlanDto>> AddPlan(PlanDto plan);
        Task<ServiceResult> UpdatePlan(PlanDto plan);
        Task<ServiceResult> DeletePlan(Guid planId);
    }
}
