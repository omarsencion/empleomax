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
    public interface IPlanServiceService: IBaseService
    {
        Task<ServiceResultList<PlanServiceSubscriptionDto>> GetAll(Category category);
        Task<ServiceResultList<PlanServiceDto>> GetByPlanId(Guid PlanId);
        Task<ServiceResult> AddPlanService(PlanServiceDto serviceDto);
        Task<ServiceResult> DeletePlanService(PlanServiceDto serviceDto);
    }
}
